import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.HashMap;
import java.util.Iterator;

public class TaxonomyTree {
    // Property
    public TaxonomyNode root = new TaxonomyNode(-1);
    public HashMap<Integer, TaxonomyNode> mapItemToTaxonomyNode = new HashMap<Integer, TaxonomyNode>();
    public int GI = 0;
    public int I = 0;
    public int maxLevel = 0;

    // Constructer
    public TaxonomyTree(String pTaxonomyFilePath) throws IOException
    {
        mapItemToTaxonomyNode.put(-1, root);

        BufferedReader reader = new BufferedReader(new InputStreamReader(new FileInputStream(pTaxonomyFilePath)));
        try {
            String line;
            while ((line = reader.readLine()) != null) {
                if (line.isEmpty() == true || line.charAt(0)=='#' || line.charAt(0)=='@') { 
					continue;
				}

                String split[] = line.split(",");
                int child = Integer.parseInt(split[0]);
                int parent = Integer.parseInt(split[1]);

                TaxonomyNode nodeChild = mapItemToTaxonomyNode.get(child);
                if(nodeChild == null) {
                    nodeChild = new TaxonomyNode(child);

                    TaxonomyNode nodeParent = mapItemToTaxonomyNode.get(parent);
                    if(nodeParent == null)
                    {
                        nodeParent = new TaxonomyNode(parent);
                        mapItemToTaxonomyNode.put(parent, nodeParent);
                    }                   
                    nodeParent.AddChild(nodeChild); 
                    nodeChild.parent = nodeParent;
                    mapItemToTaxonomyNode.put(child, nodeChild);
                }
                else {
                    TaxonomyNode nodeParent = mapItemToTaxonomyNode.get(parent);
                    if(nodeParent == null)
                    {
                        nodeParent = new TaxonomyNode(parent);
                        mapItemToTaxonomyNode.put(parent, nodeParent);
                    }                    
                    nodeChild.parent = nodeParent;
                    nodeParent.AddChild(nodeChild);
                }                
            }

        } catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if(reader != null) { 
				reader.close(); 
			}
            
        }
        Iterator<Integer> iterator = mapItemToTaxonomyNode.keySet().iterator();
        while (iterator.hasNext()) {
            Integer item = iterator.next();
            TaxonomyNode node = mapItemToTaxonomyNode.get(item);

            int currentLevel = 0;
            if (item != -1) {
                if (node.parent == null) {
                    this.root.AddChild(node);
                }
                
                currentLevel = 1;
                TaxonomyNode nodeParent = node.parent;
                while (nodeParent != null && nodeParent.data != -1) {
                    currentLevel++;
                    nodeParent = nodeParent.parent;
                }
            }

            if (node.childs.size() == 0) {
                I++;
            }
            else {
                GI++;
            }
            node.level = currentLevel;
            if (currentLevel > maxLevel) {
                maxLevel = currentLevel;
            }
        }
    }
    
}