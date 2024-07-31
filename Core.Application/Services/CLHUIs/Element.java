public class Element {
    // Property
    public int tid;
    public double nu;
    public double nru;
    public double pu;
    public int ppos;

    public double tu = 0;


    // Constructer
    public Element(int pTid, double pNU, double pNRU, double pPU, int pPPos, double pTU)
    {
        this.tid = pTid;
        this.nu = pNU;
        this.nru = pNRU;
        this.pu = pPU;
        this.ppos = pPPos;
        this.tu = pTU;
    }

    // Function
}
