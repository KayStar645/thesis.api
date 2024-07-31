import java.util.ArrayList;
import java.util.List;

public class CompactTaxUtilityList {
    // Property
    public int item;
    public double nu;
    public double nru;
    public double cu;
    public double cru;
    public double cpu;

    public List<Element> elements = new ArrayList<Element>();

    public double GWU = 0;

    
    // Constructer
    public CompactTaxUtilityList(int pItem)
    {
        this.item = pItem;
    }
    
    public CompactTaxUtilityList(int pItem, double pNu, double pNru, double pCu, double pCru, double pCpu)
    {
        this.item = pItem;
        this.nu = pNu;
        this.nru = pNru;
        this.cu = pCu;
        this.cru = pCru;
        this.cpu = pCpu;
    }

    // Function
    public void AddElement(Element pElement)
    {
        this.nu += pElement.nu;
        this.nru += pElement.nru;

        this.elements.add(pElement);

        this.GWU += pElement.tu;
    }
}
