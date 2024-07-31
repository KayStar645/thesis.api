using Core.Application.Common.Interfaces;
using System.Diagnostics;
using static Core.Domain.Entities.Order;

namespace Core.Application.Services
{
    public record Transaction
    {
        public List<int?> Item = new List<int?>();

        public long? TU { get; set; }

        public List<int?> U = new List<int?>();
    }

    public record Taxonomy
    {
        public string? Parent { get; set; }

        public string? Child { get; set; }
    }

    public record Item
    {
        public int? Id { get; set; }

        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public List<Item>? Items { get; set; }

        public bool IsCategory { get; set; }
    }

    public record CLHUIs
    {
        public int Id { get; set; }

        public List<Item>? Items { get; set; }

        public double? Profit { get; set; }
    }

    public class CLHUIService : ICLHUIService
    {
        private readonly ISupermarketDbContext _context;

        private string currentDirectory = AppDomain.CurrentDomain.BaseDirectory
            .Replace(@"UI.WebApi\bin\Debug\net8.0\", @"Core.Application\Services\CLHUIs\").ToString();
        private string transaction = "transaction.txt";
        private string taxonomy = "taxonomy.txt";
        private string output = "output.txt";

        public CLHUIService(ISupermarketDbContext pContext)
        {
            _context = pContext;
            transaction = currentDirectory + transaction;
            taxonomy = currentDirectory + taxonomy;
            output = currentDirectory + output;
        }

        public async Task<List<CLHUIs>> RunAlgorithm(int pMinUtil, int? pMonth, int? pYear)
        {
            await WriteFile(currentDirectory, pMonth, pYear);

            // Tạo tiến trình để chạy Java
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = $"AlgoCLHHMiner {pMinUtil} \"{transaction}\" \"{taxonomy}\" \"{output}\"",
                WorkingDirectory = currentDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();

                // Đọc đầu ra từ tiến trình
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    // Xử lý lỗi nếu tiến trình Java trả về mã lỗi
                    Console.WriteLine("Error running Java process:");
                    Console.WriteLine(error);
                }
                else
                {
                    // Xử lý kết quả đầu ra từ tiến trình Java
                    Console.WriteLine("Java process output:");
                    Console.WriteLine(output);
                }
            }

            // Khởi tạo đối tượng CLHUIs
            List<CLHUIs> clhuis = new List<CLHUIs>();
            int index = 0;

            // Mở file và đọc từng dòng
            try
            {
                using (StreamReader sr = new StreamReader(output))
                {
                    // Đọc dòng đầu tiên để lấy danh sách Item và Profit
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] utilParts = line.Split(new string[] { "#UTIL:" }, StringSplitOptions.RemoveEmptyEntries);
                        if (utilParts.Length >= 2)
                        {
                            string itemsPart = utilParts[0].Trim();
                            string[] itemStrings = itemsPart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            CLHUIs clh = new CLHUIs();
                            clh.Id = ++index;
                            clh.Items = new List<Item>();
                            double profit;
                            if (double.TryParse(utilParts[1].Trim(), out profit))
                            {
                                clh.Profit = profit;
                            }

                            foreach (var itemString in itemStrings)
                            {
                                Item item = new Item();
                                if (itemString.StartsWith("90"))
                                {
                                    item.IsCategory = true;
                                    item.Id = int.Parse(itemString.Substring(1));
                                    var p = await _context.Categories.FindAsync(item.Id);
                                    item.InternalCode = p.InternalCode;
                                    item.Name = p.Name;
                                }
                                else
                                {
                                    item.IsCategory = false;
                                    item.Id = int.Parse(itemString);
                                    var p = await _context.Products.FindAsync(item.Id);
                                    item.InternalCode = p.InternalCode;
                                    item.Name = p.Name;
                                }
                                clh.Items.Add(item);
                            }
                            clhuis.Add(clh);
                        }
                    }
                }
                return clhuis;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CLHUIs> Detail(int id)
        {
            int index = 0;
            CLHUIs clh = new CLHUIs();
            using (StreamReader sr = new StreamReader(output))
            {
                // Đọc dòng đầu tiên để lấy danh sách Item và Profit
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if(index < id)
                    {
                        index++;
                        continue;
                    }
                    string[] utilParts = line.Split(new string[] { "#UTIL:" }, StringSplitOptions.RemoveEmptyEntries);
                    if (utilParts.Length >= 2)
                    {
                        string itemsPart = utilParts[0].Trim();
                        string[] itemStrings = itemsPart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        clh.Items = new List<Item>();
                        double profit;
                        if (double.TryParse(utilParts[1].Trim(), out profit))
                        {
                            clh.Profit = profit;
                        }

                        foreach (var itemString in itemStrings)
                        {
                            Item item = new Item();
                            if (itemString.StartsWith("90"))
                            {
                                item.IsCategory = true;
                                item.Id = int.Parse(itemString.Substring(1));
                                var p = await _context.Categories.FindAsync(item.Id);
                                item.InternalCode = p.InternalCode;
                                item.Name = p.Name;
                            }
                            else
                            {
                                item.IsCategory = false;
                                item.Id = int.Parse(itemString);
                                var p = await _context.Products.FindAsync(item.Id);
                                item.InternalCode = p.InternalCode;
                                item.Name = p.Name;
                            }
                            clh.Items.Add(item);
                        }
                    }
                    break;
                }
            }

            for (int j = 0; j < clh.Items.Count; j++)
            {
                clh.Items[j] = await GetChild(clh.Items[j]);
            }
            return clh;
        }

        private async Task<Item> GetProducts(Item pItem)
        {
            if (pItem.IsCategory)
            {
                var categories = await _context.Categories
                    .Where(x => x.ParentId == pItem.Id)
                    .ToListAsync();
                foreach (var category in categories)
                {
                    var item = new Item()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        IsCategory = true,
                    };
                    await GetChild(item);
                }
                var products = await _context.Products
                    .Where(x => x.CategoryId == pItem.Id)
                    .ToListAsync();
                foreach (var product in products)
                {
                    var item = new Item()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        IsCategory = false,
                    };
                    if (pItem.Items == null)
                    {
                        pItem.Items = new List<Item>();
                    }
                    pItem.Items.Add(item);
                }
            }

            return pItem;
        }

        private async Task<Item> GetChild(Item pItem)
        {
            if(pItem.IsCategory)
            {
                var categories = await _context.Categories
                    .Where(x => x.ParentId == pItem.Id)
                    .ToListAsync();
                foreach(var category in categories)
                {
                    var item = new Item()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        IsCategory = true,
                    };
                    if(pItem.Items == null)
                    {
                        pItem.Items = new List<Item>();
                    }
                    pItem.Items.Add(item);
                    await GetChild(item);
                }
                var products = await _context.Products
                    .Where(x => x.CategoryId == pItem.Id)
                    .ToListAsync();
                foreach (var product in products)
                {
                    var item = new Item()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        IsCategory = false,
                    };
                    if (pItem.Items == null)
                    {
                        pItem.Items = new List<Item>();
                    }
                    pItem.Items.Add(item);
                }
            }

            return pItem;
        }

        private async Task WriteFile(string pFilePath, int? pMonth, int? pYear)
        {
            var query = _context.DetailOrders
                .Where(x => x.Order.Status == OrderStatus.Transport ||
                            x.Order.Status == OrderStatus.Received)
                .AsQueryable();
            if(pMonth != null)
            {
                query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                        x.Order.UpdatedAt.Value.Year == pYear &&
                                        x.Order.UpdatedAt.Value.Month == pMonth);
            }
            else if (pYear != null)
            {
                query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                        x.Order.UpdatedAt.Value.Year == pYear);
            }

            var transactions = await query
                .GroupBy(x => x.OrderId)
                .Select(g => new Transaction
                {
                    Item = g.Select(x => x.ProductId).ToList(),
                    TU = g.Sum(x => (long?)x.Profit),
                    U = g.Select(x => (int?)x.Profit).ToList()
                })
                .ToListAsync();

            // Chuyển đổi danh sách transactions thành chuỗi định dạng
            var lines = transactions.Select(t =>
            {
                var itemsPart = string.Join(" ", t.Item);
                var profitPart = $"{t.TU}:{string.Join(" ", t.U)}";
                return $"{itemsPart}:{profitPart}";
            });

            // Ghi chuỗi định dạng vào file
            await File.WriteAllLinesAsync(pFilePath + "transaction.txt", lines);

            var categories = await _context.Categories
                .Where(x => x.Parent != null)
                .Select(x => new Taxonomy
                {
                    Parent = "90" + x.ParentId.ToString(),
                    Child = "90" + x.Id.ToString()
                }).ToListAsync();

            var products = await _context.Products
                .Where(x => x.Parent == null)
                .Select(x => new Taxonomy
                {
                    Parent = "90" + x.CategoryId.ToString(),
                    Child = x.Id.ToString()
                }).ToListAsync();

            var taxonomyList = categories.Concat(products).ToList();

            var lines2 = taxonomyList.Select(t => $"{t.Child},{t.Parent}");

            await File.WriteAllLinesAsync(pFilePath + "taxonomy.txt", lines2);
        }
    }
}
