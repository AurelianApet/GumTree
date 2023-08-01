namespace GumTree
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.list_photo = new System.Windows.Forms.ListBox();
            this.list_product = new System.Windows.Forms.ListBox();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.tb_passwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_phone = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_title = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_category = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.bt_clean = new System.Windows.Forms.Button();
            this.bt_post = new System.Windows.Forms.Button();
            this.bt_postall = new System.Windows.Forms.Button();
            this.tb_description = new System.Windows.Forms.TextBox();
            this.lb_stat = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.bt_view = new System.Windows.Forms.Button();
            this.list_posted = new System.Windows.Forms.ListBox();
            this.bt_edit = new System.Windows.Forms.Button();
            this.bt_delete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.bt_deleteAd = new System.Windows.Forms.Button();
            this.bt_add = new System.Windows.Forms.Button();
            this.c = new System.Windows.Forms.NumericUpDown();
            this.pg_bar = new System.Windows.Forms.ProgressBar();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.bt_stop = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tb_time = new System.Windows.Forms.NumericUpDown();
            this.tb_relog = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.c)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_relog)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(19, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "USERNAME";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(226, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "PASSWORD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Location = new System.Drawing.Point(478, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "PHOTOS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Location = new System.Drawing.Point(700, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Adverts List";
            // 
            // list_photo
            // 
            this.list_photo.BackColor = System.Drawing.Color.Silver;
            this.list_photo.ForeColor = System.Drawing.Color.Black;
            this.list_photo.FormattingEnabled = true;
            this.list_photo.HorizontalScrollbar = true;
            this.list_photo.Location = new System.Drawing.Point(425, 123);
            this.list_photo.Name = "list_photo";
            this.list_photo.Size = new System.Drawing.Size(191, 251);
            this.list_photo.TabIndex = 10;
            this.list_photo.TabStop = false;
            // 
            // list_product
            // 
            this.list_product.BackColor = System.Drawing.Color.Silver;
            this.list_product.ForeColor = System.Drawing.Color.Black;
            this.list_product.FormattingEnabled = true;
            this.list_product.HorizontalScrollbar = true;
            this.list_product.Location = new System.Drawing.Point(653, 122);
            this.list_product.Name = "list_product";
            this.list_product.Size = new System.Drawing.Size(191, 251);
            this.list_product.TabIndex = 11;
            this.list_product.TabStop = false;
            this.list_product.SelectedIndexChanged += new System.EventHandler(this.list_product_SelectedIndexChanged);
            // 
            // tb_username
            // 
            this.tb_username.BackColor = System.Drawing.Color.Silver;
            this.tb_username.ForeColor = System.Drawing.Color.Black;
            this.tb_username.Location = new System.Drawing.Point(18, 144);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(156, 20);
            this.tb_username.TabIndex = 1;
            // 
            // tb_passwd
            // 
            this.tb_passwd.BackColor = System.Drawing.Color.Silver;
            this.tb_passwd.ForeColor = System.Drawing.Color.Black;
            this.tb_passwd.Location = new System.Drawing.Point(224, 144);
            this.tb_passwd.Name = "tb_passwd";
            this.tb_passwd.Size = new System.Drawing.Size(156, 20);
            this.tb_passwd.TabIndex = 2;
            this.tb_passwd.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(19, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "PHONE";
            // 
            // tb_phone
            // 
            this.tb_phone.BackColor = System.Drawing.Color.Silver;
            this.tb_phone.ForeColor = System.Drawing.Color.Black;
            this.tb_phone.Location = new System.Drawing.Point(18, 194);
            this.tb_phone.Name = "tb_phone";
            this.tb_phone.Size = new System.Drawing.Size(156, 20);
            this.tb_phone.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label7.Location = new System.Drawing.Point(18, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "TITLE";
            // 
            // tb_title
            // 
            this.tb_title.BackColor = System.Drawing.Color.Silver;
            this.tb_title.ForeColor = System.Drawing.Color.Black;
            this.tb_title.Location = new System.Drawing.Point(18, 245);
            this.tb_title.Name = "tb_title";
            this.tb_title.Size = new System.Drawing.Size(361, 20);
            this.tb_title.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label8.Location = new System.Drawing.Point(19, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "CATEGORY";
            // 
            // tb_category
            // 
            this.tb_category.BackColor = System.Drawing.Color.Silver;
            this.tb_category.ForeColor = System.Drawing.Color.Black;
            this.tb_category.Location = new System.Drawing.Point(19, 295);
            this.tb_category.Name = "tb_category";
            this.tb_category.Size = new System.Drawing.Size(156, 20);
            this.tb_category.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label9.Location = new System.Drawing.Point(19, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "DESCRIPTION";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label10.Location = new System.Drawing.Point(22, 448);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 20);
            this.label10.TabIndex = 0;
            this.label10.Text = "PRICE";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label11.Location = new System.Drawing.Point(227, 448);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 20);
            this.label11.TabIndex = 0;
            this.label11.Text = "TIME";
            // 
            // bt_clean
            // 
            this.bt_clean.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_clean.ForeColor = System.Drawing.Color.Blue;
            this.bt_clean.Location = new System.Drawing.Point(425, 379);
            this.bt_clean.Name = "bt_clean";
            this.bt_clean.Size = new System.Drawing.Size(191, 28);
            this.bt_clean.TabIndex = 12;
            this.bt_clean.Text = "CLEAN";
            this.bt_clean.UseVisualStyleBackColor = true;
            this.bt_clean.Click += new System.EventHandler(this.bt_clean_Click);
            // 
            // bt_post
            // 
            this.bt_post.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_post.ForeColor = System.Drawing.Color.Blue;
            this.bt_post.Location = new System.Drawing.Point(653, 410);
            this.bt_post.Name = "bt_post";
            this.bt_post.Size = new System.Drawing.Size(89, 95);
            this.bt_post.TabIndex = 12;
            this.bt_post.Text = "POST";
            this.bt_post.UseVisualStyleBackColor = true;
            this.bt_post.Click += new System.EventHandler(this.bt_post_Click);
            // 
            // bt_postall
            // 
            this.bt_postall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_postall.ForeColor = System.Drawing.Color.Blue;
            this.bt_postall.Location = new System.Drawing.Point(755, 410);
            this.bt_postall.Name = "bt_postall";
            this.bt_postall.Size = new System.Drawing.Size(89, 95);
            this.bt_postall.TabIndex = 12;
            this.bt_postall.Text = "POST ALL";
            this.bt_postall.UseVisualStyleBackColor = true;
            this.bt_postall.Click += new System.EventHandler(this.bt_postall_Click);
            // 
            // tb_description
            // 
            this.tb_description.BackColor = System.Drawing.Color.Silver;
            this.tb_description.ForeColor = System.Drawing.Color.Black;
            this.tb_description.Location = new System.Drawing.Point(19, 346);
            this.tb_description.Multiline = true;
            this.tb_description.Name = "tb_description";
            this.tb_description.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_description.Size = new System.Drawing.Size(360, 96);
            this.tb_description.TabIndex = 13;
            // 
            // lb_stat
            // 
            this.lb_stat.AutoSize = true;
            this.lb_stat.BackColor = System.Drawing.Color.Transparent;
            this.lb_stat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lb_stat.Location = new System.Drawing.Point(207, 513);
            this.lb_stat.Name = "lb_stat";
            this.lb_stat.Size = new System.Drawing.Size(0, 13);
            this.lb_stat.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label12.Location = new System.Drawing.Point(918, 98);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(131, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "Posted Adverts";
            // 
            // bt_view
            // 
            this.bt_view.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_view.ForeColor = System.Drawing.Color.Blue;
            this.bt_view.Location = new System.Drawing.Point(884, 379);
            this.bt_view.Name = "bt_view";
            this.bt_view.Size = new System.Drawing.Size(189, 28);
            this.bt_view.TabIndex = 15;
            this.bt_view.Text = "VIEW POSTED ADS";
            this.bt_view.UseVisualStyleBackColor = true;
            this.bt_view.Click += new System.EventHandler(this.bt_view_Click);
            // 
            // list_posted
            // 
            this.list_posted.BackColor = System.Drawing.Color.Silver;
            this.list_posted.ForeColor = System.Drawing.Color.Black;
            this.list_posted.FormattingEnabled = true;
            this.list_posted.HorizontalScrollbar = true;
            this.list_posted.Location = new System.Drawing.Point(884, 122);
            this.list_posted.Name = "list_posted";
            this.list_posted.Size = new System.Drawing.Size(189, 251);
            this.list_posted.TabIndex = 16;
            this.list_posted.TabStop = false;
            this.list_posted.SelectedIndexChanged += new System.EventHandler(this.list_posted_SelectedIndexChanged);
            // 
            // bt_edit
            // 
            this.bt_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_edit.ForeColor = System.Drawing.Color.Blue;
            this.bt_edit.Location = new System.Drawing.Point(884, 410);
            this.bt_edit.Name = "bt_edit";
            this.bt_edit.Size = new System.Drawing.Size(85, 65);
            this.bt_edit.TabIndex = 17;
            this.bt_edit.Text = "EDIT";
            this.bt_edit.UseVisualStyleBackColor = true;
            this.bt_edit.Click += new System.EventHandler(this.bt_edit_Click_1);
            // 
            // bt_delete
            // 
            this.bt_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_delete.ForeColor = System.Drawing.Color.Blue;
            this.bt_delete.Location = new System.Drawing.Point(988, 410);
            this.bt_delete.Name = "bt_delete";
            this.bt_delete.Size = new System.Drawing.Size(85, 65);
            this.bt_delete.TabIndex = 17;
            this.bt_delete.Text = "DELETE";
            this.bt_delete.UseVisualStyleBackColor = true;
            this.bt_delete.Click += new System.EventHandler(this.bt_delete_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Blue;
            this.button1.Location = new System.Drawing.Point(425, 410);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 65);
            this.button1.TabIndex = 18;
            this.button1.Text = "Add Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Blue;
            this.button2.Location = new System.Drawing.Point(521, 410);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 65);
            this.button2.TabIndex = 18;
            this.button2.Text = "Remove Image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bt_deleteAd
            // 
            this.bt_deleteAd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_deleteAd.ForeColor = System.Drawing.Color.Blue;
            this.bt_deleteAd.Location = new System.Drawing.Point(654, 379);
            this.bt_deleteAd.Name = "bt_deleteAd";
            this.bt_deleteAd.Size = new System.Drawing.Size(190, 28);
            this.bt_deleteAd.TabIndex = 19;
            this.bt_deleteAd.Text = "Delete Ad";
            this.bt_deleteAd.UseVisualStyleBackColor = true;
            this.bt_deleteAd.Click += new System.EventHandler(this.bt_deleteAd_Click);
            // 
            // bt_add
            // 
            this.bt_add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_add.ForeColor = System.Drawing.Color.Blue;
            this.bt_add.Location = new System.Drawing.Point(425, 478);
            this.bt_add.Name = "bt_add";
            this.bt_add.Size = new System.Drawing.Size(191, 26);
            this.bt_add.TabIndex = 20;
            this.bt_add.Text = "Add/Change ad";
            this.bt_add.UseVisualStyleBackColor = true;
            this.bt_add.Click += new System.EventHandler(this.bt_add_Click);
            // 
            // c
            // 
            this.c.BackColor = System.Drawing.Color.Silver;
            this.c.ForeColor = System.Drawing.Color.Black;
            this.c.Location = new System.Drawing.Point(19, 473);
            this.c.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(156, 20);
            this.c.TabIndex = 21;
            // 
            // pg_bar
            // 
            this.pg_bar.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.pg_bar.Location = new System.Drawing.Point(427, 510);
            this.pg_bar.Name = "pg_bar";
            this.pg_bar.Size = new System.Drawing.Size(646, 19);
            this.pg_bar.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label13.Location = new System.Drawing.Point(110, 513);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "STATUS:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label15.Location = new System.Drawing.Point(226, 170);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 20);
            this.label15.TabIndex = 0;
            this.label15.Text = "Relog";
            // 
            // bt_stop
            // 
            this.bt_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_stop.ForeColor = System.Drawing.Color.Blue;
            this.bt_stop.Location = new System.Drawing.Point(884, 478);
            this.bt_stop.Name = "bt_stop";
            this.bt_stop.Size = new System.Drawing.Size(189, 26);
            this.bt_stop.TabIndex = 25;
            this.bt_stop.Text = "STOP";
            this.bt_stop.UseVisualStyleBackColor = true;
            this.bt_stop.Click += new System.EventHandler(this.bt_relog_Click);
            // 
            // button3
            // 
            this.button3.Image = global::GumTree.Properties.Resources.mini;
            this.button3.Location = new System.Drawing.Point(1099, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(15, 14);
            this.button3.TabIndex = 26;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::GumTree.Properties.Resources.cancel;
            this.button4.Image = global::GumTree.Properties.Resources.cancel;
            this.button4.Location = new System.Drawing.Point(1118, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(15, 14);
            this.button4.TabIndex = 27;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tb_time
            // 
            this.tb_time.BackColor = System.Drawing.Color.Silver;
            this.tb_time.ForeColor = System.Drawing.Color.Black;
            this.tb_time.Location = new System.Drawing.Point(223, 473);
            this.tb_time.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.tb_time.Name = "tb_time";
            this.tb_time.Size = new System.Drawing.Size(156, 20);
            this.tb_time.TabIndex = 21;
            // 
            // tb_relog
            // 
            this.tb_relog.BackColor = System.Drawing.Color.Silver;
            this.tb_relog.ForeColor = System.Drawing.Color.Black;
            this.tb_relog.Location = new System.Drawing.Point(224, 195);
            this.tb_relog.Maximum = new decimal(new int[] {
            -1486618625,
            232830643,
            0,
            0});
            this.tb_relog.Name = "tb_relog";
            this.tb_relog.Size = new System.Drawing.Size(156, 20);
            this.tb_relog.TabIndex = 21;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GumTree.Properties.Resources.Background;
            this.ClientSize = new System.Drawing.Size(1145, 550);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.bt_stop);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.pg_bar);
            this.Controls.Add(this.tb_relog);
            this.Controls.Add(this.tb_time);
            this.Controls.Add(this.c);
            this.Controls.Add(this.bt_add);
            this.Controls.Add(this.bt_deleteAd);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bt_delete);
            this.Controls.Add(this.bt_edit);
            this.Controls.Add(this.list_posted);
            this.Controls.Add(this.bt_view);
            this.Controls.Add(this.lb_stat);
            this.Controls.Add(this.tb_description);
            this.Controls.Add(this.bt_postall);
            this.Controls.Add(this.bt_post);
            this.Controls.Add(this.bt_clean);
            this.Controls.Add(this.tb_passwd);
            this.Controls.Add(this.tb_category);
            this.Controls.Add(this.tb_title);
            this.Controls.Add(this.tb_phone);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.list_product);
            this.Controls.Add(this.list_photo);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "GumTree";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.c)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_relog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox list_photo;
        private System.Windows.Forms.ListBox list_product;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.TextBox tb_passwd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_phone;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_title;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_category;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button bt_clean;
        private System.Windows.Forms.Button bt_post;
        private System.Windows.Forms.Button bt_postall;
        private System.Windows.Forms.TextBox tb_description;
        private System.Windows.Forms.Label lb_stat;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button bt_view;
        private System.Windows.Forms.ListBox list_posted;
        private System.Windows.Forms.Button bt_edit;
        private System.Windows.Forms.Button bt_delete;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bt_deleteAd;
        private System.Windows.Forms.Button bt_add;
        private System.Windows.Forms.NumericUpDown c;
        private System.Windows.Forms.ProgressBar pg_bar;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button bt_stop;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.NumericUpDown tb_time;
        private System.Windows.Forms.NumericUpDown tb_relog;
    }
}

