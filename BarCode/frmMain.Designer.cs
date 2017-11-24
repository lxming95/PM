using System;
namespace BarCode
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("人员信息新增");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("人员信息管理");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("人员信息管理", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("人员信息导入");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("人员信息查询导出");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("操作员设置");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("操作员管理", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("人员信息管理", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("信息导入导出", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("操作员管理", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("操作员设置", 5);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("人员信息新增", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("人员信息管理", 2);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("人员信息导入", 4);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("人员信息查询导出", 3);
            this.picBanner = new System.Windows.Forms.PictureBox();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.imgNode = new System.Windows.Forms.ImageList(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.bsiServer = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.bsiDateTime = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.bsiUser = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.nbgSys = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.trvSystemSet = new System.Windows.Forms.TreeView();
            this.nbcMenu = new DevExpress.XtraNavBar.NavBarControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.tcMain = new DevExpress.XtraTab.XtraTabControl();
            this.tpMain = new DevExpress.XtraTab.XtraTabPage();
            this.lstMain = new System.Windows.Forms.ListView();
            this.panelForm = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.navBarGroupControlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbcMenu)).BeginInit();
            this.nbcMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).BeginInit();
            this.tcMain.SuspendLayout();
            this.tpMain.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBanner
            // 
            this.picBanner.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picBanner.BackgroundImage")));
            this.picBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.picBanner.Location = new System.Drawing.Point(0, 0);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(929, 51);
            this.picBanner.TabIndex = 3;
            this.picBanner.TabStop = false;
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "43.png");
            this.imgList.Images.SetKeyName(1, "PersionAdd.png");
            this.imgList.Images.SetKeyName(2, "InfManage.png");
            this.imgList.Images.SetKeyName(3, "导出查询结果.png");
            this.imgList.Images.SetKeyName(4, "导入客户信息.png");
            this.imgList.Images.SetKeyName(5, "操作员管理 .png");
            // 
            // imgNode
            // 
            this.imgNode.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgNode.ImageStream")));
            this.imgNode.TransparentColor = System.Drawing.Color.Transparent;
            this.imgNode.Images.SetKeyName(0, "bullet_white.png");
            this.imgNode.Images.SetKeyName(1, "bullet_green.png");
            this.imgNode.Images.SetKeyName(2, "bullet_red.png");
            // 
            // timer
            // 
            this.timer.Interval = 60000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1,
            this.bsiServer,
            this.barStaticItem2,
            this.bsiDateTime,
            this.barStaticItem3,
            this.bsiUser});
            this.barManager1.MaxItemId = 8;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "状态栏";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiServer),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiDateTime),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem3),
            new DevExpress.XtraBars.LinkPersistInfo(this.bsiUser)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "状态栏";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "服务器：";
            this.barStaticItem1.Id = 2;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // bsiServer
            // 
            this.bsiServer.Caption = "服务器";
            this.bsiServer.Id = 3;
            this.bsiServer.Name = "bsiServer";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "当前日期：";
            this.barStaticItem2.Id = 4;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // bsiDateTime
            // 
            this.bsiDateTime.Caption = "当前时间";
            this.bsiDateTime.Id = 5;
            this.bsiDateTime.Name = "bsiDateTime";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "操作员：";
            this.barStaticItem3.Id = 6;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // bsiUser
            // 
            this.bsiUser.Caption = "操作员";
            this.bsiUser.Id = 7;
            this.bsiUser.Name = "bsiUser";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(929, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 459);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(929, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 459);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(929, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 459);
            // 
            // nbgSys
            // 
            this.nbgSys.Caption = "功能区";
            this.nbgSys.ControlContainer = this.navBarGroupControlContainer2;
            this.nbgSys.Expanded = true;
            this.nbgSys.GroupClientHeight = 227;
            this.nbgSys.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.nbgSys.Name = "nbgSys";
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.navBarGroupControlContainer2.Appearance.Options.UseBackColor = true;
            this.navBarGroupControlContainer2.Controls.Add(this.trvSystemSet);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(169, 304);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // trvSystemSet
            // 
            this.trvSystemSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvSystemSet.ImageIndex = 1;
            this.trvSystemSet.ImageList = this.imgNode;
            this.trvSystemSet.ItemHeight = 24;
            this.trvSystemSet.Location = new System.Drawing.Point(0, 0);
            this.trvSystemSet.Name = "trvSystemSet";
            treeNode1.ImageIndex = 1;
            treeNode1.Name = "nodeAdd";
            treeNode1.Text = "人员信息新增";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "nodePersion";
            treeNode2.Text = "人员信息管理";
            treeNode3.Name = "nodeDetailed";
            treeNode3.Text = "人员信息管理";
            treeNode4.Name = "nodeInfIn";
            treeNode4.Text = "人员信息导入";
            treeNode5.Name = "nodeInfOut";
            treeNode5.Text = "人员信息查询导出";
            treeNode6.ImageIndex = 1;
            treeNode6.Name = "nodeUserSet";
            treeNode6.Tag = "BarCode.FrmUserSet|操作员权限|user.png";
            treeNode6.Text = "操作员设置";
            treeNode7.Name = "nodeUserPower";
            treeNode7.Text = "操作员管理";
            this.trvSystemSet.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode7});
            this.trvSystemSet.SelectedImageIndex = 1;
            this.trvSystemSet.Size = new System.Drawing.Size(169, 304);
            this.trvSystemSet.TabIndex = 2;
            this.trvSystemSet.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvSystemSet_NodeMouseDoubleClick);
            // 
            // nbcMenu
            // 
            this.nbcMenu.ActiveGroup = this.nbgSys;
            this.nbcMenu.Controls.Add(this.navBarGroupControlContainer2);
            this.nbcMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.nbcMenu.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.nbgSys});
            this.nbcMenu.Location = new System.Drawing.Point(0, 0);
            this.nbcMenu.Name = "nbcMenu";
            this.nbcMenu.OptionsNavPane.ExpandedWidth = 169;
            this.nbcMenu.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.nbcMenu.Size = new System.Drawing.Size(169, 408);
            this.nbcMenu.TabIndex = 4;
            this.nbcMenu.Text = "navBarControl1";
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(169, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 408);
            this.splitterControl1.TabIndex = 6;
            this.splitterControl1.TabStop = false;
            // 
            // tcMain
            // 
            this.tcMain.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(174, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedTabPage = this.tpMain;
            this.tcMain.Size = new System.Drawing.Size(755, 408);
            this.tcMain.TabIndex = 7;
            this.tcMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpMain});
            this.tcMain.CloseButtonClick += new System.EventHandler(this.tcMain_CloseButtonClick);
            this.tcMain.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.tcMain_ControlRemoved);
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add(this.lstMain);
            this.tpMain.Name = "tpMain";
            this.tpMain.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.tpMain.Size = new System.Drawing.Size(749, 379);
            this.tpMain.Text = "主窗体";
            // 
            // lstMain
            // 
            this.lstMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstMain.BackgroundImage")));
            this.lstMain.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "人员信息管理";
            listViewGroup1.Name = "人员信息管理";
            listViewGroup2.Header = "信息导入导出";
            listViewGroup2.Name = "信息导入导出";
            listViewGroup3.Header = "操作员管理";
            listViewGroup3.Name = "操作员管理";
            this.lstMain.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            listViewItem1.Group = listViewGroup3;
            listViewItem2.Group = listViewGroup1;
            listViewItem3.Group = listViewGroup1;
            listViewItem4.Group = listViewGroup2;
            listViewItem5.Group = listViewGroup2;
            this.lstMain.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lstMain.LargeImageList = this.imgList;
            this.lstMain.Location = new System.Drawing.Point(0, 0);
            this.lstMain.Name = "lstMain";
            this.lstMain.Size = new System.Drawing.Size(749, 379);
            this.lstMain.TabIndex = 1;
            this.lstMain.UseCompatibleStateImageBehavior = false;
            this.lstMain.ItemActivate += new System.EventHandler(this.lstMain_ItemActivate);
            // 
            // panelForm
            // 
            this.panelForm.Controls.Add(this.tcMain);
            this.panelForm.Controls.Add(this.splitterControl1);
            this.panelForm.Controls.Add(this.nbcMenu);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 51);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(929, 408);
            this.panelForm.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 486);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.picBanner);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "人员信息管理系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.navBarGroupControlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nbcMenu)).EndInit();
            this.nbcMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).EndInit();
            this.tcMain.ResumeLayout(false);
            this.tpMain.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBanner;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ImageList imgNode;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem bsiServer;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem bsiDateTime;
        private DevExpress.XtraBars.BarStaticItem barStaticItem3;
        private DevExpress.XtraBars.BarStaticItem bsiUser;
        private System.Windows.Forms.Panel panelForm;
        private DevExpress.XtraTab.XtraTabControl tcMain;
        private DevExpress.XtraTab.XtraTabPage tpMain;
        public System.Windows.Forms.ListView lstMain;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraNavBar.NavBarControl nbcMenu;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer2;
        public System.Windows.Forms.TreeView trvSystemSet;
        private DevExpress.XtraNavBar.NavBarGroup nbgSys;
    }
}

