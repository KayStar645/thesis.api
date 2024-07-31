import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.Stack;

public class AlgoCLHHMiner {

    private int minUtil;
    private TaxonomyTree taxonomy;


    private Map<Integer, Double> mapItemToGWU = new HashMap<Integer, Double>();
    private static Map<Integer, CompactTaxUtilityList> mapItemTo_CTU_List = new HashMap<Integer, CompactTaxUtilityList>();

    Map<Set<Integer>, Double> mapEUCS = new HashMap<>();

    int taxDepth = 0;
	int giCount = 0;
	int itemCount = 0;
    Set<Integer> itemInDB = new HashSet<Integer>();

    double startTimestamp = 0;
	double endTimestamp = 0;
	public double maxMemory = 0;

    BufferedWriter writer;
	int countCLHUI = 0;
	int candidate = 0;

    class Pair {
		int item = 0;
		double utility = 0;
        double utilityRemove = 0;

        public Pair() { }

        public Pair(int pItem, double pUtility) {
            this.item = pItem;
            this.utility = pUtility;
        }
	}


	List<List<Pair>> datasetAfterRemove = new ArrayList<List<Pair>>();;

    public void printStats() throws IOException {
		System.out.println(" Runtime time ~ : " + (endTimestamp - startTimestamp) + " ms");
        System.out.println(" Max Memory ~ " + MemoryLogger.getInstance().getMaxMemory() + " MB");
		System.out.println(" Cross level high utility itemsets (count): " + countCLHUI);
		System.out.println("   Number of items              : " + itemCount);
		System.out.println("   Number of generalized items             : " + giCount);
		System.out.println("   Taxonomy depth   : " + taxDepth);
		System.out.println("   Candidates (count): " + candidate);
		System.out.println("======================================");
	}


    
    

    
    

    
    

    public static void main(String[] args) throws IOException {
        
        if (args.length != 4) {
            System.out.println("Usage: java AlgoCLHHMiner <pMinUtil> <transactionFilePath> <taxonomyFilePath> <resultFilePath>");
            return;
        }

        
        int pMinUtil = Integer.parseInt(args[0]);
        String transactionFilePath = args[1];
        String taxonomyFilePath = args[2];
        String resultFilePath = args[3];

        
        AlgoCLHHMiner clh = new AlgoCLHHMiner();
        clh.runAlgorithm(pMinUtil, transactionFilePath, taxonomyFilePath, resultFilePath);

    }

    @SuppressWarnings("resource")
    public void runAlgorithm(int pMinUtil, String pTransactionFilePath,
            String pTaxonomyFilePath, String pResultFilePath) throws IOException {

        startTimestamp = System.currentTimeMillis();

        this.minUtil = pMinUtil;
        taxonomy = new TaxonomyTree(pTaxonomyFilePath);

        writer = new BufferedWriter(new FileWriter(pResultFilePath));

        BufferedReader reader = new BufferedReader(new InputStreamReader(new FileInputStream(new File(pTransactionFilePath))));

        String line;
        while ((line = reader.readLine()) != null) {
            if (line.isEmpty() == true || line.charAt(0) == '#' ||
                line.charAt(0) == '%' || line.charAt(0) == '@') {
                continue;
            }

            String split[] = line.split(":");
            String items[] = split[0].split(" ");
            double tu = Double.parseDouble(split[1]);

            for(int i = 0; i < items.length; i++) {
                int item = Integer.parseInt(items[i]);
                itemInDB.add(item);

                Double gwu = mapItemToGWU.get(item);
                gwu = (gwu == null) ? tu : gwu + tu;
                mapItemToGWU.put(item, gwu);

                if(gwu >= minUtil) {
                    CompactTaxUtilityList tuList = new CompactTaxUtilityList(item);
                    mapItemTo_CTU_List.put(item, tuList);
                }

                if (taxonomy.mapItemToTaxonomyNode.get(item) == null) {
                    TaxonomyNode newNode = new TaxonomyNode(item);
                    taxonomy.mapItemToTaxonomyNode.get(-1).AddChild(newNode);
                    taxonomy.mapItemToTaxonomyNode.put(item, newNode);
                } else {
                    TaxonomyNode parentNode = taxonomy.mapItemToTaxonomyNode.get(item).parent;
                    while (parentNode.data != -1) {
                        int parent = parentNode.data;
                        Double gwuP = mapItemToGWU.get(parent);
                        gwuP = (gwuP == null) ? tu : gwuP + tu;
                        mapItemToGWU.put(parent, gwuP);

                        if(gwuP >= minUtil) {
                            CompactTaxUtilityList tuList = new CompactTaxUtilityList(parent);
                            mapItemTo_CTU_List.put(parent, tuList);
                        }

                        parentNode = parentNode.parent;
                    }
                }
            }
        }

        List<CompactTaxUtilityList> CTU_Lists = new ArrayList<>(mapItemTo_CTU_List.values());
        Collections.sort(CTU_Lists, new Comparator<CompactTaxUtilityList>() {
            public int compare(CompactTaxUtilityList o1, CompactTaxUtilityList o2) {
                return compareItems(o1.item, o2.item);
            }
        });

        reader = new BufferedReader(new InputStreamReader(new FileInputStream(new File(pTransactionFilePath))));
        int tid = 0;
        while ((line = reader.readLine()) != null) {
            if (line.isEmpty() == true || line.charAt(0) == '#' ||
                line.charAt(0) == '%' || line.charAt(0) == '@') {
                continue;
            }

            String split[] = line.split(":");
            String items[] = split[0].split(" ");
            double tu = Double.parseDouble(split[1]);
            String utilityValues[] = split[2].split(" ");

            double remainingUtility = tu;

            HashMap<Integer, Pair> mapItemToPair = new HashMap<Integer, Pair>();

            for(int i = 0; i < items.length; i++) {
                int item = Integer.parseInt(items[i]);
                double utility = Double.parseDouble(utilityValues[i]);

                boolean isRemove = false;
                if(mapItemToGWU.get(item) >= minUtil) {
                    mapItemToPair.put(item, new Pair(item, utility));
                }
                else {
                    isRemove = true;
                }

                TaxonomyNode nodeParent = taxonomy.mapItemToTaxonomyNode.get(item).parent;
                while (nodeParent.data != -1) {
                    Pair pairOfParent = mapItemToPair.get(nodeParent.data);
                    double utilityOfParent = 0;

                    if(mapItemToGWU.get(nodeParent.data) >= minUtil) {
                        utilityOfParent = pairOfParent == null ? utility : pairOfParent.utility + utility;
                        Pair pair = new Pair(nodeParent.data, utilityOfParent);
                        if(isRemove) {
                            pair.utilityRemove = pairOfParent == null ? utility : pairOfParent.utilityRemove + utility;
                        }
                        mapItemToPair.put(nodeParent.data, pair);
                    }
                    nodeParent = nodeParent.parent;
                }
            }

            List<Pair> newRevisedTransaction = new ArrayList<>(mapItemToPair.values());
            Collections.sort(newRevisedTransaction, new Comparator<Pair>() {
                public int compare(Pair o1, Pair o2) {
                    return compareItems(o1.item, o2.item);
                }
            });

            for (int i = 0; i < newRevisedTransaction.size(); i++) {
                Pair pair = newRevisedTransaction.get(i);
                Boolean isGI = taxonomy.mapItemToTaxonomyNode.get(mapItemToPair.get(pair.item).item).childs.size() != 0;

                int PPOS = -1;
                if (i != newRevisedTransaction.size() - 1) {
                    PPOS = mapItemTo_CTU_List.get(newRevisedTransaction.get(i + 1).item).elements.size();
                }

                if(isGI) {
                    double countUtilityOfEachItem = remainingUtility;
                    for (int j = i + 1; j < newRevisedTransaction.size(); j++) {
                        Pair pairAfter = newRevisedTransaction.get(j);
                        Boolean isGI2 = taxonomy.mapItemToTaxonomyNode.get(mapItemToPair.get(pairAfter.item).item).childs.size() != 0;
                        Boolean isParent = checkParent(pair.item, pairAfter.item);

                        if(!isParent) {
                            
                            Double gwuSum = mapEUCS.get(new HashSet<>(Set.of(pair.item, pairAfter.item)));
                            double gwuEUCS = gwuSum == null ? tu : gwuSum + tu;
                            mapEUCS.put(new HashSet<>(Set.of(pair.item, pairAfter.item)), gwuEUCS);
                        }

                        if(isGI2) {
                            continue;
                        }

                        if (isParent) {
                            countUtilityOfEachItem -= pairAfter.utility;
                        }
                    }
                    CompactTaxUtilityList utilityListOfItem = mapItemTo_CTU_List.get(pair.item);
                    if (utilityListOfItem != null) {
                        double re = PPOS == -1 ? 0 : countUtilityOfEachItem;
                        Element element = new Element(tid, pair.utility, re, 0, PPOS, tu);
                        utilityListOfItem.AddElement(element);
                    }

                } else {
                    for (int j = i + 1; j < newRevisedTransaction.size(); j++) {
                        Pair pairAfter = newRevisedTransaction.get(j);
                        Boolean isParent = checkParent(pair.item, pairAfter.item);
                        if(!isParent) {
                            
                            Double gwuSum = mapEUCS.get(new HashSet<>(Set.of(pair.item, pairAfter.item)));
                            double gwuEUCS = gwuSum == null ? tu : gwuSum + tu;
                            mapEUCS.put(new HashSet<>(Set.of(pair.item, pairAfter.item)), gwuEUCS);
                        }
                    }

                    remainingUtility = remainingUtility - pair.utility;

                    CompactTaxUtilityList utilityListOfItem = mapItemTo_CTU_List.get(pair.item);
                    double re = PPOS == -1 ? 0 : remainingUtility;
                    Element element = new Element(tid, pair.utility, re, 0, PPOS, tu);
                    utilityListOfItem.AddElement(element);
                }

            }
			datasetAfterRemove.add(newRevisedTransaction);
            tid++;
        }
		checkMemory();

        itemCount = itemInDB.size();
		giCount = taxonomy.GI;
		taxDepth = taxonomy.maxLevel;

        MemoryLogger.getInstance().checkMemory();

        
        
        
        
        
        
        

        ExploreSearchTree(new int[0], CTU_Lists);

		endTimestamp = System.currentTimeMillis();
		reader.close();
		writer.close();

        MemoryLogger.getInstance().checkMemory();

    }

    private void ExploreSearchTree(int[] pPrefix, List<CompactTaxUtilityList> pCTUs) throws IOException {
        for(int i = 0; i < pCTUs.size(); i++) {
            CompactTaxUtilityList X = pCTUs.get(i);

            int[] sorted_prefix = new int[pPrefix.length + 1];
			System.arraycopy(pPrefix, 0, sorted_prefix, 0, pPrefix.length);
			sorted_prefix[pPrefix.length] = X.item;

            candidate++;
            if (X.nu + X.cu >= minUtil) {
                writeOut(pPrefix, X.item, X.nu + X.cu, false);
            }

            if (X.nu + X.cu + X.nru + X.cru >= minUtil) {
                List<CompactTaxUtilityList> exCTULs = new ArrayList<>();
                BitSet itemRemove = new BitSet();

                for (int j = i + 1; j < pCTUs.size(); j++) {
                    CompactTaxUtilityList exCTU = pCTUs.get(j);
                    Boolean isParent = checkParent(X.item, exCTU.item);

                    if (!isParent) {
                        Double gwuSum = mapEUCS.get(new HashSet<>(Set.of(X.item, exCTU.item)));

                        if (gwuSum != null && gwuSum < minUtil) {
                            Stack<TaxonomyNode> stack = new Stack<>();
                            stack.addAll(taxonomy.mapItemToTaxonomyNode.get(exCTU.item).childs);

                            while (!stack.isEmpty()) {
                                TaxonomyNode child = stack.pop();
                                itemRemove.set(child.data);
                                stack.addAll(child.childs);
                            }
                            continue;
                        }

                        if (!itemRemove.get(exCTU.item)) {
                            exCTULs.add(exCTU);
                        }
                    }
                }

                List<CompactTaxUtilityList> exCTUs = ConstructCTU(X, exCTULs, sorted_prefix.length);
                ExploreSearchTree(sorted_prefix, exCTUs);
            }

        }
        MemoryLogger.getInstance().checkMemory();
    }

    private List<CompactTaxUtilityList> ConstructCTU(CompactTaxUtilityList pX, List<CompactTaxUtilityList> pCTUs, int pLength) {
        List<CompactTaxUtilityList> exCTUs = new ArrayList<CompactTaxUtilityList>();
        List<Double> pruneLA = new ArrayList<Double>();
        List<Integer> eyTid = new ArrayList<Integer>();

        int newSize = pCTUs.size();
        for(int i = 0; i < pCTUs.size(); i++) {
            CompactTaxUtilityList uList = new CompactTaxUtilityList(pCTUs.get(i).item);
			exCTUs.add(uList);
            pruneLA.add(pX.cu + pX.cru + pX.nu + pX.nru);
            eyTid.add(0);
            if (pLength > 1) {
                exCTUs.get(i).cu += pCTUs.get(i).cu + pX.cu - pX.cpu;
                exCTUs.get(i).cru += pCTUs.get(i).cru;
                exCTUs.get(i).cpu += pX.cu;
            }
        }

        HashMap<List<Integer>, Integer> duplicate = new HashMap<List<Integer>, Integer>();
		List<Integer> location = null;
		for (Element ex : pX.elements) {
			location = new ArrayList<Integer>();
			for (int j = 0; j < pCTUs.size(); j++) {
                CompactTaxUtilityList ext = exCTUs.get(j);
				if (ext == null)
					continue;

				List<Element> eylist = pCTUs.get(j).elements;

				while (eyTid.get(j) < eylist.size() && eylist.get(eyTid.get(j)).tid < ex.tid) {
					eyTid.set(j, eyTid.get(j) + 1);
				}

				if (eyTid.get(j) < eylist.size() && eylist.get(eyTid.get(j)).tid == ex.tid) {
                    location.add(j);
                }
				else
				{
					pruneLA.set(j, pruneLA.get(j) - ex.nu - ex.nru);
					if (pruneLA.get(j) < minUtil) {
						exCTUs.set(j, null);
                        newSize--;
					}
				}
			}

			if (location.size() == newSize)
			{
				UpdateClosed(pCTUs, exCTUs, location, ex, eyTid);
			} else
			{
				if (location.size() == 0)
					continue;
				long remainingUtility = 0;

				if (!duplicate.containsKey(location))
                {
                    duplicate.put(location, exCTUs.get(location.get(location.size() - 1)).elements.size());

                    for (int i = location.size() - 1; i >= 0; i--) {
                        CompactTaxUtilityList CULListOfItem = exCTUs.get(location.get(i));
                        Element Y = pCTUs.get(location.get(i)).elements.get(eyTid.get(location.get(i)));

                        int ppos = i > 0 ? exCTUs.get(location.get(i - 1)).elements.size() : -1;

                        Element element = new Element(ex.tid, ex.nu + Y.nu - ex.pu, remainingUtility, ex.nu, ppos, Y.tu);

                        CULListOfItem.AddElement(element);
                        remainingUtility += Y.nu - ex.pu;
                    }

                } else
                {
                    int dupPos = duplicate.get(location);
                    UpdateElement(pX, pCTUs, exCTUs, location, ex, dupPos, eyTid);
                }
			}
		}
        exCTUs.removeIf(cul -> cul == null);
		return exCTUs;
    }

    private void UpdateClosed(List<CompactTaxUtilityList> pCTUs,List<CompactTaxUtilityList> pExCTUs,
                            List<Integer> pLocation, Element pEx, List<Integer> pEyTid) {
		double nru = 0;
		for (int j = pLocation.size() - 1; j >= 0; j--) {
			CompactTaxUtilityList ey = pCTUs.get(pLocation.get(j));
			Element eyy = ey.elements.get(pEyTid.get(pLocation.get(j)));

			pExCTUs.get(pLocation.get(j)).cu += pEx.nu + eyy.nu - pEx.pu;

			pExCTUs.get(pLocation.get(j)).cru += nru;
			pExCTUs.get(pLocation.get(j)).cpu += pEx.nu;
			nru = nru + eyy.nu - pEx.pu;
		}
	}

    private void UpdateElement(CompactTaxUtilityList pX, List<CompactTaxUtilityList> pCTUs, List<CompactTaxUtilityList> pExCTUs,
                            List<Integer> pLocation, Element pEx, int pDupPos, List<Integer> pEyTid) {
		double nru = 0;
		int pos = pDupPos;
		for (int j = pLocation.size() - 1; j >= 0; j--) {
			CompactTaxUtilityList ey = pCTUs.get(pLocation.get(j));
			Element eyy = ey.elements.get(pEyTid.get(pLocation.get(j)));

			pExCTUs.get(pLocation.get(j)).elements.get(pos).nu += pEx.nu + eyy.nu - pEx.pu;
			pExCTUs.get(pLocation.get(j)).nu += pEx.nu + eyy.nu - pEx.pu;
			pExCTUs.get(pLocation.get(j)).elements.get(pos).nru += nru;
			pExCTUs.get(pLocation.get(j)).nru += nru;
			pExCTUs.get(pLocation.get(j)).elements.get(pos).pu += pEx.nu;
			nru = nru + eyy.nu - pEx.pu;

			pos = pExCTUs.get(pLocation.get(j)).elements.get(pos).ppos;

		}
	}

    private void writeOut(int[] pPrefix, int pItem, double pUtility, boolean pTest) throws IOException {
		countCLHUI++;
		StringBuilder buffer = new StringBuilder();
		
		for (int i = 0; i < pPrefix.length; i++) {
			buffer.append(pPrefix[i]);
			buffer.append(' ');
		}
		buffer.append(pItem);
        if(pTest) {
            buffer.append(" #TEST: ");
        } else {
            buffer.append(" #UTIL: ");
        }
		buffer.append(pUtility);

		
		writer.write(buffer.toString());
		writer.newLine();
	}

    private boolean checkParent(int item1, int item2) {
		TaxonomyNode nodeItem1 = taxonomy.mapItemToTaxonomyNode.get(item1);
		TaxonomyNode nodeItem2 = taxonomy.mapItemToTaxonomyNode.get(item2);
		int levelOfItem1 = nodeItem1.level;
		int levelOfItem2 = nodeItem2.level;
		if (levelOfItem1 == levelOfItem2) {
			return false;
		} else {
			if (levelOfItem1 > levelOfItem2) {
				TaxonomyNode parentItem1 = nodeItem1.parent;
				while (parentItem1.data != -1) {
					if (parentItem1.data == nodeItem2.data) {
						return true;
					}
					parentItem1 = parentItem1.parent;
				}
				return false;
			} else {
				TaxonomyNode parentItem2 = nodeItem2.parent;
				while (parentItem2.data != -1) {
					if (parentItem2.data == nodeItem1.data) {
						return true;
					}
					parentItem2 = parentItem2.parent;
				}
				return false;
			}
		}
	}

    private int compareItems(int item1, int item2) {
		int levelOfItem1 = taxonomy.mapItemToTaxonomyNode.get(item1).level;
		int levelOfItem2 = taxonomy.mapItemToTaxonomyNode.get(item2).level;
		if (levelOfItem1 == levelOfItem2) {
			int compare = (int) (mapItemToGWU.get(item1) - mapItemToGWU.get(item2));
			return (compare == 0) ? item1 - item2 : compare;
		} else {
			return levelOfItem1 - levelOfItem2;
		}
	}

    private String abc(int item) {
        switch (item) {
            case 1:
                return "a";
            case 2:
                return "b";
            case 3:
                return "c";
            case 4:
                return "d";
            case 5:
                return "e";
            case 6:
                return "f";
            case 7:
                return "g";
            case 8:
                return "W";
            case 9:
                return "X";
            case 10:
                return "Y";
            case 11:
                return "Z";

            default:
                return "XXX";
        }
    }

    private void checkMemory() {
		double currentMemory = (Runtime.getRuntime().totalMemory() - Runtime.getRuntime().freeMemory()) / 1024d / 1024d;
		if (currentMemory > maxMemory) {
			maxMemory = currentMemory;
		}
	}
}
