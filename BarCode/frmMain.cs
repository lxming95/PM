using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Drawing;
using System.Windows.Forms;
using PersonnelManagement;

namespace BarCode
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {

        #region 属性
        public string getUserName
        {
            get
            {
                return bsiUser.Caption;
            }
        }
        #endregion
        
        /// <summary>
        /// Banner初始化
        /// </summary>
        private void InitBanner()
        {
            picBanner.Image = Image.FromFile(Application.StartupPath + @"\Images\Banner.jpg");
            picBanner.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Images\BannerBG.jpg");
            picBanner.Height = 80;
        }

        public frmMain()
        {
            //初始化登陆后的主界面
            InitializeComponent();
            //模块许可HashTable初始化
            //LicenseInit();

            //Banner初始化
            InitBanner();
            

            Pub.PubValue.frmmain = this;

            //显示登录窗体
            FrmLogin frmlogin = new FrmLogin(this);
            frmlogin.ShowDialog();
            
            
            //状态栏赋值
            bsiServer.Caption = Pub.PubValue.Server;
            bsiDateTime.Caption = Pub.PubValue.LoginDate.ToString("yyyy-MM-dd");
            bsiUser.Caption = Pub.PubValue.UserName;
            
        }
        
        private void tabPage_Resize(object sender, EventArgs e)
        {
            if (tcMain.TabPages.Count == 1)
            {
                return;
            }
            if (((DevExpress.XtraTab.XtraTabPage)sender).Controls.Count == 0)
            {
                return;
            }
            //重设子窗体大小
            ((DevExpress.XtraEditors.XtraForm)((DevExpress.XtraTab.XtraTabPage)sender).Controls[0]).WindowState = FormWindowState.Normal;
            ((DevExpress.XtraTab.XtraTabPage)sender).Controls[0].Top = 0;
            ((DevExpress.XtraTab.XtraTabPage)sender).Controls[0].Left = 0;
            ((DevExpress.XtraTab.XtraTabPage)sender).Controls[0].Width = ((DevExpress.XtraTab.XtraTabPage)sender).Width;
            ((DevExpress.XtraTab.XtraTabPage)sender).Controls[0].Height = ((DevExpress.XtraTab.XtraTabPage)sender).Height;
        }
        
        /// <summary>
        /// 检查标签页是否被打开
        /// </summary>
        /// <param name="tab">窗体空间</param>
        /// <param name="tabName">要打开的功能的标签的名称</param>
        /// <returns></returns>
        public bool tabControlCheckHave(XtraTabControl tab, String tabName)
        {
            for (int i = 0; i < tab.TabPages.Count; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    tab.SelectedTabPageIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            bsiDateTime.Caption = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void lstMain_ItemActivate(object sender, EventArgs e)
        {
            //MessageBox.Show(lstMain.SelectedItems[0].Text);
            string frmname = lstMain.SelectedItems[0].Text;
            Form form=new Form();
            form = caseFromName(frmname);



            //判断是否已经显示
            //如果已经显示，则选择该页签
            //如果没有显示，则创建该页签
            if (tabControlCheckHave(tcMain, frmname))
            {
                return;
            }
            else
            {
                XtraTabPage tp = new XtraTabPage();
                tp.Text = frmname;
                tp.Resize += new System.EventHandler(this.tabPage_Resize);
                tcMain.TabPages.Add(tp);
                tcMain.SelectedTabPageIndex = tcMain.TabPages.Count - 1;
                tcMain.SelectedTabPage.Controls.Clear();
                tcMain.SelectedTabPage.Controls.Add(form);
                form.Show();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //退出系统前提示
            if (XtraMessageBox.Show("是否要退出本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                //foreach (XtraTabPage tp in tcMain.TabPages)
                for (int tpID = 0; tpID < tcMain.TabPages.Count; tpID++)
                {
                    XtraTabPage tp = tcMain.TabPages[tpID];

                    if (tp.Name == "tpMain")
                    {
                        continue;
                    }
                    tcMain.TabPages.Remove(tp);
                    tpID--;
                }
                e.Cancel = false;
                //Application.ExitThread();
            }
        }
        
        

        private void trvSystemSet_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            string frmname = e.Node.Text;
            Form form = new Form();
            form = caseFromName(frmname);

            //判断是否已经显示
            //如果已经显示，则选择该页签
            //如果没有显示，则创建该页签
            if (tabControlCheckHave(tcMain, frmname))
            {
                return;
            }
            else
            {
                XtraTabPage tp = new XtraTabPage();
                tp.Text = frmname;
                tp.Resize += new System.EventHandler(this.tabPage_Resize);
                tcMain.TabPages.Add(tp);
                tcMain.SelectedTabPageIndex = tcMain.TabPages.Count - 1;
                tcMain.SelectedTabPage.Controls.Clear();
                tcMain.SelectedTabPage.Controls.Add(form);
                form.Show();
            }
        }
        

        private void tcMain_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control.Controls.Count == 0)
            {
                return;
            }

            ((XtraForm)e.Control.Controls[0]).Close();
        }

        private void tcMain_CloseButtonClick(object sender, EventArgs e)
        {
            tcMain.TabPages.Remove(tcMain.HotTrackedTabPage);
        }
        /// <summary>
        /// 判断打开窗体的名字
        /// </summary>
        /// <param name="frmname"></param>
        /// <returns></returns>
        private Form caseFromName(string frmname)
        {
            Form form = new Form();
            if (frmname == "操作员设置")
            {
                form = new FrmUserSet();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            if (frmname == "人员信息管理")
            {
                form = new FrmPersion();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            if (frmname == "人员信息新增")
            {
                form = new FrmInfMain();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            if (frmname == "人员信息查询导出")
            {
                form = new FrmExport();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            if (frmname == "人员信息导入")
            {
                form = new FrmInport();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            if (frmname == "执行SQL语句")
            {
                form = new FrmInport();
                // 窗体设置
                form.TopLevel = false;
                form.WindowState = FormWindowState.Maximized;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            return form;
        }
    }
}
