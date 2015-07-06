﻿using AntShares.Core;
using AntShares.Wallets;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnWalletChanged()
        {
            修改密码CToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            交易TToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            listView1.Items.Clear();
            if (Program.CurrentWallet != null)
            {
                listView1.Items.AddRange(Program.CurrentWallet.GetAddresses().Select(p => new ListViewItem(new string[] { p.ToAddress() })).ToArray());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.LocalNode.StartSynchronize(Program.Blockchain);
        }

        private void 创建钱包数据库NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CreateWalletDialog dialog = new CreateWalletDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Program.CurrentWallet = UserWallet.CreateDatabase(dialog.WalletPath, dialog.Password);
            }
            OnWalletChanged();
        }

        private void 打开钱包数据库OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenWalletDialog dialog = new OpenWalletDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Program.CurrentWallet = UserWallet.OpenDatabase(dialog.WalletPath, dialog.Password);
            }
            OnWalletChanged();
        }

        private void 修改密码CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //弹出对话框，验证原密码，保存新密码
        }

        private void 签名SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SigningDialog dialog = new SigningDialog())
            {
                dialog.ShowDialog();
            }
        }

        private void 资产分发IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: 对已登记的资产进行分发操作
            //1. 检索当前钱包所有账户，找出登记过的所有资产；
            //2. 用户选择一种资产，并设置分发数量等；
            //3. 检查是否符合规则，如是否超过总量、分发方式是否符合约定等；
            //4. 构造交易，签名；
            //5. 广播交易
        }

        private void 官网WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://weangel.com/AntShares");
        }

        private void 开发人员工具TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helper.Show<DeveloperToolsForm>();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            显示详情DToolStripMenuItem.Enabled = listView1.SelectedIndices.Count == 1;
            复制到剪贴板CToolStripMenuItem.Enabled = listView1.SelectedIndices.Count == 1;
        }

        private void 显示详情DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WalletEntry entry = Program.CurrentWallet.GetEntry(listView1.SelectedItems[0].Text.ToScriptHash());
            using (AccountDetailsDialog dialog = new AccountDetailsDialog(entry))
            {
                dialog.ShowDialog();
            }
        }

        private void 复制到剪贴板CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].Text);
        }
    }
}