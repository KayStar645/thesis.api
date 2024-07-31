using Core.Application.Features.Base.Records;
using Core.Application.Models.Common;
using Newtonsoft.Json;

namespace Core.Application.Profiles
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<string, List<string>>().ConvertUsing<StringToListTypeConverter>();
            CreateMap<List<string>, string>().ConvertUsing<ListToStringTypeConverter>();

            CreateMap<CardImage, string>().ConvertUsing<CardImageToStringConverter>();
            CreateMap<string, CardImage>().ConvertUsing<StringToCardImageConverter>();

            //CreateMap<string, FileDto>().ConvertUsing<StringToFileDtoConverter>();
            CreateMap<FileDto, string>().ConvertUsing<FileDtoToStringConvert>();

            //CreateMap<string, List<FileDto>>().ConvertUsing<StringToFileDtoListConverter>();
            CreateMap<List<FileDto>, string>().ConvertUsing<FileDtoListToStringConvert>();

            CreateMap<Option, string>().ConvertUsing<OptionToStringTypeConverter>();
            CreateMap<string, Option>().ConvertUsing<StringToOptionTypeConverter>();

            CreateMap<List<Extra>, string>().ConvertUsing<ExtraToStringTypeConverter>();
            CreateMap<string, List<Extra>>().ConvertUsing<StringToExtraTypeConverter>();


        }
    }

    #region Mapping string và list<string>
    public class StringToListTypeConverter : ITypeConverter<string, List<string>>
    {
        public List<string> Convert(string source, List<string> destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<string>();
            }

            return source.Split(',').ToList();
        }
    }

    public class ListToStringTypeConverter : ITypeConverter<List<string>, string>
    {
        public string Convert(List<string> source, string destination, ResolutionContext context)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            return string.Join(",", source);
        }
    }
    #endregion

    #region Mapping CardImage và string
    public class CardImageToStringConverter : ITypeConverter<CardImage, string>
    {
        public string Convert(CardImage source, string destination, ResolutionContext context)
        {
            return $"{source.Front},{source.Back}";
        }
    }

    public class StringToCardImageConverter : ITypeConverter<string, CardImage>
    {
        public CardImage Convert(string source, CardImage destination, ResolutionContext context)
        {
            var parts = source.Split(',');

            return new CardImage
            {
                Front = parts.Length > 0 ? parts[0] : null,
                Back = parts.Length > 1 ? parts[1] : null
            };
        }
    }

    #endregion

    #region Mapping string và FileDto
    //public class StringToFileDtoConverter : ITypeConverter<string, FileDto>
    //{
    //    public FileDto Convert(string source, FileDto destination, ResolutionContext context)
    //    {
    //        if (string.IsNullOrEmpty(source))
    //        {
    //            return new FileDto();
    //        }

    //        var result = _googleDriveService.GetFileInfoFromGoogleDrive(source).Result;
    //        return result.Data ?? new FileDto();
    //    }
    //}

    public class FileDtoToStringConvert : ITypeConverter<FileDto, string>
    {
        public string Convert(FileDto source, string destination, ResolutionContext context)
        {
            return source?.Path ?? string.Empty;
        }
    }

    //public class StringToFileDtoListConverter : ITypeConverter<string, List<FileDto>>
    //{
    //    public List<FileDto> Convert(string source, List<FileDto> destination, ResolutionContext context)
    //    {
    //        if (string.IsNullOrEmpty(source))
    //        {
    //            return new List<FileDto>();
    //        }

    //        var imagePaths = source.Split(',');
    //        var fileDtos = new List<FileDto>();

    //        foreach (var imagePath in imagePaths)
    //        {
    //            var result = _googleDriveService.GetFileInfoFromGoogleDrive(imagePath).Result;
    //            if (result.Data != null)
    //            {
    //                fileDtos.Add(result.Data);
    //            }
    //        }

    //        return fileDtos;
    //    }
    //}

    public class FileDtoListToStringConvert : ITypeConverter<List<FileDto>, string>
    {
        public string Convert(List<FileDto> source, string destination, ResolutionContext context)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            var paths = source.Select(fileDto => fileDto.Path);
            return string.Join(",", paths);
        }
    }
    #endregion

    #region Otion và string
    public class OptionToStringTypeConverter : ITypeConverter<Option, string>
    {
        public string Convert(Option source, string destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }

            var optionValue = new
            {
                Name = source.Name,
                Select = source.Select?.ToString(),
                Value = source.Value != null ? string.Join(",", source.Value) : null
            };

            return JsonConvert.SerializeObject(optionValue);
        }
    }

    public class StringToOptionTypeConverter : ITypeConverter<string, Option>
    {
        public Option Convert(string source, Option destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var optionValue = JsonConvert.DeserializeObject<Option>(source);
            return optionValue;
        }
    }

    #endregion

    #region List<Extra> và string
    public class ExtraToStringTypeConverter : ITypeConverter<List<Extra>, string>
    {
        public string Convert(List<Extra> source, string destination, ResolutionContext context)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            var extraValue = source.Select(e => new
            {
                Name = e.Name,
                Extras = e.Extras != null ? e.Extras.Select(ex => new { Name = ex.Name, Description = ex.Description }).ToList() : null
            });

            return JsonConvert.SerializeObject(extraValue);
        }
    }

    public class StringToExtraTypeConverter : ITypeConverter<string, List<Extra>>
    {
        public List<Extra> Convert(string source, List<Extra> destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            var extraValue = JsonConvert.DeserializeObject<List<Extra>>(source);
            return extraValue;
        }
    }

    #endregion
}
