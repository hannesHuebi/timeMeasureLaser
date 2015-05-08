namespace ZeitmessungArduino
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für Designerunterstützung -
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.comboBox1 = this.Factory.CreateRibbonComboBox();
            this.buttonConnect = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.buttonMeasure1 = this.Factory.CreateRibbonButton();
            this.buttonMeasure2 = this.Factory.CreateRibbonButton();
            this.tab2 = this.Factory.CreateRibbonTab();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.tab2.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.comboBox1);
            this.group1.Items.Add(this.buttonConnect);
            this.group1.Label = "Connect";
            this.group1.Name = "group1";
            // 
            // comboBox1
            // 
            ribbonDropDownItemImpl1.Label = "COM1";
            ribbonDropDownItemImpl2.Label = "COM2";
            ribbonDropDownItemImpl3.Label = "COM3";
            ribbonDropDownItemImpl4.Label = "TEST";
            this.comboBox1.Items.Add(ribbonDropDownItemImpl1);
            this.comboBox1.Items.Add(ribbonDropDownItemImpl2);
            this.comboBox1.Items.Add(ribbonDropDownItemImpl3);
            this.comboBox1.Items.Add(ribbonDropDownItemImpl4);
            this.comboBox1.Label = "COM-Port";
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Text = null;
            this.comboBox1.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.comboBox1_TextChanged);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Label = "Connect";
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.ShowImage = true;
            this.buttonConnect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonConnect_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.buttonMeasure1);
            this.group2.Items.Add(this.buttonMeasure2);
            this.group2.Label = "Measure";
            this.group2.Name = "group2";
            // 
            // buttonMeasure1
            // 
            this.buttonMeasure1.Label = "Measure1";
            this.buttonMeasure1.Name = "buttonMeasure1";
            this.buttonMeasure1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonMeasure1_Click);
            // 
            // buttonMeasure2
            // 
            this.buttonMeasure2.Label = "Measure2";
            this.buttonMeasure2.Name = "buttonMeasure2";
            this.buttonMeasure2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonMeasure2_Click);
            // 
            // tab2
            // 
            this.tab2.Label = "tab2";
            this.tab2.Name = "tab2";
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Tabs.Add(this.tab2);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.tab2.ResumeLayout(false);
            this.tab2.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBox1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonConnect;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonMeasure1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonMeasure2;
        private Microsoft.Office.Tools.Ribbon.RibbonTab tab2;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
