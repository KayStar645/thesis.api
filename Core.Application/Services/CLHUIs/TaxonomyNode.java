import java.util.ArrayList;
import java.util.List;

public class TaxonomyNode {
    // Property
    public int data;
    public TaxonomyNode parent = null;
    public List<TaxonomyNode> childs = new ArrayList<TaxonomyNode>();
    public int level;

     // Constructer
     public TaxonomyNode(int pData)
     {
         this.data = pData;
     }

    // Function
    public void AddChild(TaxonomyNode pChild)
    {
        pChild.parent = this;
        this.childs.add(pChild);
    }
}
