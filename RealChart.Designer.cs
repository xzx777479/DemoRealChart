namespace DemoSharp
{
    partial class RealChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RealChart));
            this.btnStart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnStop = new System.Windows.Forms.Button();
            this.AI0_textBox = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Datasave_Box = new System.Windows.Forms.CheckBox();
            this.Autoscale_Box = new System.Windows.Forms.CheckBox();
            this.Ymax_txtBox1 = new System.Windows.Forms.TextBox();
            this.Ymin_txtBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Ymin_txtBox2 = new System.Windows.Forms.TextBox();
            this.Ymax_txtBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Ymin_txtBox3 = new System.Windows.Forms.TextBox();
            this.Ymax_txtBox3 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(621, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始测量";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chart1
            // 
            this.chart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            chartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Gray;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(100, 73);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(820, 700);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(721, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止测量";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // AI0_textBox
            // 
            this.AI0_textBox.Location = new System.Drawing.Point(721, 41);
            this.AI0_textBox.Name = "AI0_textBox";
            this.AI0_textBox.Size = new System.Drawing.Size(75, 21);
            this.AI0_textBox.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(82, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(141, 20);
            this.comboBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "串口选择";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point(82, 47);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(141, 20);
            this.comboBox2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = "波特率";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.Location = new System.Drawing.Point(240, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "串口扫描";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Datasave_Box
            // 
            this.Datasave_Box.AutoSize = true;
            this.Datasave_Box.Location = new System.Drawing.Point(621, 46);
            this.Datasave_Box.Margin = new System.Windows.Forms.Padding(2);
            this.Datasave_Box.Name = "Datasave_Box";
            this.Datasave_Box.Size = new System.Drawing.Size(78, 16);
            this.Datasave_Box.TabIndex = 12;
            this.Datasave_Box.Text = "Save Data";
            this.Datasave_Box.UseVisualStyleBackColor = true;
            // 
            // Autoscale_Box
            // 
            this.Autoscale_Box.AutoSize = true;
            this.Autoscale_Box.Checked = true;
            this.Autoscale_Box.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Autoscale_Box.Location = new System.Drawing.Point(502, 46);
            this.Autoscale_Box.Margin = new System.Windows.Forms.Padding(2);
            this.Autoscale_Box.Name = "Autoscale_Box";
            this.Autoscale_Box.Size = new System.Drawing.Size(84, 16);
            this.Autoscale_Box.TabIndex = 14;
            this.Autoscale_Box.Text = "Auto Scale";
            this.Autoscale_Box.UseVisualStyleBackColor = true;
            // 
            // Ymax_txtBox1
            // 
            this.Ymax_txtBox1.Location = new System.Drawing.Point(53, 104);
            this.Ymax_txtBox1.Name = "Ymax_txtBox1";
            this.Ymax_txtBox1.Size = new System.Drawing.Size(41, 21);
            this.Ymax_txtBox1.TabIndex = 15;
            // 
            // Ymin_txtBox1
            // 
            this.Ymin_txtBox1.Location = new System.Drawing.Point(53, 150);
            this.Ymin_txtBox1.Name = "Ymin_txtBox1";
            this.Ymin_txtBox1.Size = new System.Drawing.Size(41, 21);
            this.Ymin_txtBox1.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Y MAX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 15);
            this.label4.TabIndex = 18;
            this.label4.Text = "Y MIN";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 399);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 15);
            this.label5.TabIndex = 22;
            this.label5.Text = "Y MIN";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 353);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 21;
            this.label6.Text = "Y MAX";
            // 
            // Ymin_txtBox2
            // 
            this.Ymin_txtBox2.Location = new System.Drawing.Point(53, 396);
            this.Ymin_txtBox2.Name = "Ymin_txtBox2";
            this.Ymin_txtBox2.Size = new System.Drawing.Size(41, 21);
            this.Ymin_txtBox2.TabIndex = 20;
            // 
            // Ymax_txtBox2
            // 
            this.Ymax_txtBox2.Location = new System.Drawing.Point(53, 350);
            this.Ymax_txtBox2.Name = "Ymax_txtBox2";
            this.Ymax_txtBox2.Size = new System.Drawing.Size(41, 21);
            this.Ymax_txtBox2.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 648);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 15);
            this.label7.TabIndex = 26;
            this.label7.Text = "Y MIN";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 602);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 15);
            this.label8.TabIndex = 25;
            this.label8.Text = "Y MAX";
            // 
            // Ymin_txtBox3
            // 
            this.Ymin_txtBox3.Location = new System.Drawing.Point(53, 645);
            this.Ymin_txtBox3.Name = "Ymin_txtBox3";
            this.Ymin_txtBox3.Size = new System.Drawing.Size(41, 21);
            this.Ymin_txtBox3.TabIndex = 24;
            // 
            // Ymax_txtBox3
            // 
            this.Ymax_txtBox3.Location = new System.Drawing.Point(53, 599);
            this.Ymax_txtBox3.Name = "Ymax_txtBox3";
            this.Ymax_txtBox3.Size = new System.Drawing.Size(41, 21);
            this.Ymax_txtBox3.TabIndex = 23;
            // 
            // RealChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 783);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Ymin_txtBox3);
            this.Controls.Add(this.Ymax_txtBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Ymin_txtBox2);
            this.Controls.Add(this.Ymax_txtBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Ymin_txtBox1);
            this.Controls.Add(this.Ymax_txtBox1);
            this.Controls.Add(this.Autoscale_Box);
            this.Controls.Add(this.Datasave_Box);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.AI0_textBox);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RealChart";
            this.Text = "ECG_Plot";
            this.Load += new System.EventHandler(this.RealChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox AI0_textBox;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox Datasave_Box;
        private System.Windows.Forms.CheckBox Autoscale_Box;
        private System.Windows.Forms.TextBox Ymax_txtBox1;
        private System.Windows.Forms.TextBox Ymin_txtBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Ymin_txtBox2;
        private System.Windows.Forms.TextBox Ymax_txtBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Ymin_txtBox3;
        private System.Windows.Forms.TextBox Ymax_txtBox3;
    }
}

