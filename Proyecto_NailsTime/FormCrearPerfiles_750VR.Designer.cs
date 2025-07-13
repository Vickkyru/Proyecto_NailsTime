namespace Proyecto_NailsTime
{
    partial class FormCrearPerfiles
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
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbfamperf = new System.Windows.Forms.ComboBox();
            this.cmbpermperf = new System.Windows.Forms.ComboBox();
            this.cmbpermfam = new System.Windows.Forms.ComboBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtfam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtnomperf = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbfamfam = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.agperf = new System.Windows.Forms.Button();
            this.btnelimperf = new System.Windows.Forms.Button();
            this.btnelimfam = new System.Windows.Forms.Button();
            this.btnagfam = new System.Windows.Forms.Button();
            this.elimpermfam = new System.Windows.Forms.Button();
            this.agpermfam = new System.Windows.Forms.Button();
            this.btnelimfamfam = new System.Windows.Forms.Button();
            this.btnagfamfam = new System.Windows.Forms.Button();
            this.btnelimpermperf = new System.Windows.Forms.Button();
            this.btnagpermperf = new System.Windows.Forms.Button();
            this.btnagfamperf = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbperf = new System.Windows.Forms.ComboBox();
            this.cmbfam = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnmodperf = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(399, 171);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(217, 266);
            this.treeView2.TabIndex = 1;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(125, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Gestion de perfiles";
            // 
            // cmbfamperf
            // 
            this.cmbfamperf.FormattingEnabled = true;
            this.cmbfamperf.Location = new System.Drawing.Point(19, 311);
            this.cmbfamperf.Name = "cmbfamperf";
            this.cmbfamperf.Size = new System.Drawing.Size(121, 21);
            this.cmbfamperf.TabIndex = 4;
            this.cmbfamperf.SelectedIndexChanged += new System.EventHandler(this.cmbfamperf_SelectedIndexChanged);
            // 
            // cmbpermperf
            // 
            this.cmbpermperf.FormattingEnabled = true;
            this.cmbpermperf.Location = new System.Drawing.Point(12, 176);
            this.cmbpermperf.Name = "cmbpermperf";
            this.cmbpermperf.Size = new System.Drawing.Size(121, 21);
            this.cmbpermperf.TabIndex = 7;
            this.cmbpermperf.SelectedIndexChanged += new System.EventHandler(this.cmbpermperf_SelectedIndexChanged);
            // 
            // cmbpermfam
            // 
            this.cmbpermfam.FormattingEnabled = true;
            this.cmbpermfam.Location = new System.Drawing.Point(651, 176);
            this.cmbpermfam.Name = "cmbpermfam";
            this.cmbpermfam.Size = new System.Drawing.Size(121, 21);
            this.cmbpermfam.TabIndex = 10;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(170, 171);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(217, 266);
            this.treeView1.TabIndex = 18;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Familia ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Permisos";
            // 
            // txtfam
            // 
            this.txtfam.Location = new System.Drawing.Point(688, 60);
            this.txtfam.Name = "txtfam";
            this.txtfam.Size = new System.Drawing.Size(100, 20);
            this.txtfam.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(685, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Familia ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Perfil";
            // 
            // txtnomperf
            // 
            this.txtnomperf.Location = new System.Drawing.Point(15, 61);
            this.txtnomperf.Name = "txtnomperf";
            this.txtnomperf.Size = new System.Drawing.Size(100, 20);
            this.txtnomperf.TabIndex = 23;
            this.txtnomperf.TextChanged += new System.EventHandler(this.txtnomperf_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(636, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Permisos";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(636, 313);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Familia ";
            // 
            // cmbfamfam
            // 
            this.cmbfamfam.FormattingEnabled = true;
            this.cmbfamfam.Location = new System.Drawing.Point(651, 329);
            this.cmbfamfam.Name = "cmbfamfam";
            this.cmbfamfam.Size = new System.Drawing.Size(121, 21);
            this.cmbfamfam.TabIndex = 27;
            this.cmbfamfam.SelectedIndexChanged += new System.EventHandler(this.cmbfamfam_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(177, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Perfiles";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(407, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Familias";
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(376, 9);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 32;
            this.button13.Text = "Volver";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // agperf
            // 
            this.agperf.BackColor = System.Drawing.Color.RosyBrown;
            this.agperf.FlatAppearance.BorderSize = 0;
            this.agperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.agperf.Location = new System.Drawing.Point(131, 58);
            this.agperf.Name = "agperf";
            this.agperf.Size = new System.Drawing.Size(92, 23);
            this.agperf.TabIndex = 37;
            this.agperf.Text = "Agregar perfil";
            this.agperf.UseVisualStyleBackColor = false;
            this.agperf.Click += new System.EventHandler(this.agperf_Click);
            // 
            // btnelimperf
            // 
            this.btnelimperf.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelimperf.FlatAppearance.BorderSize = 0;
            this.btnelimperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelimperf.Location = new System.Drawing.Point(131, 87);
            this.btnelimperf.Name = "btnelimperf";
            this.btnelimperf.Size = new System.Drawing.Size(92, 23);
            this.btnelimperf.TabIndex = 38;
            this.btnelimperf.Text = "Eliminar";
            this.btnelimperf.UseVisualStyleBackColor = false;
            this.btnelimperf.Click += new System.EventHandler(this.btnelimperf_Click);
            // 
            // btnelimfam
            // 
            this.btnelimfam.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelimfam.FlatAppearance.BorderSize = 0;
            this.btnelimfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelimfam.Location = new System.Drawing.Point(697, 89);
            this.btnelimfam.Name = "btnelimfam";
            this.btnelimfam.Size = new System.Drawing.Size(75, 23);
            this.btnelimfam.TabIndex = 40;
            this.btnelimfam.Text = "Eliminar";
            this.btnelimfam.UseVisualStyleBackColor = false;
            this.btnelimfam.Click += new System.EventHandler(this.btnelimfam_Click);
            // 
            // btnagfam
            // 
            this.btnagfam.BackColor = System.Drawing.Color.RosyBrown;
            this.btnagfam.FlatAppearance.BorderSize = 0;
            this.btnagfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnagfam.Location = new System.Drawing.Point(584, 57);
            this.btnagfam.Name = "btnagfam";
            this.btnagfam.Size = new System.Drawing.Size(82, 23);
            this.btnagfam.TabIndex = 39;
            this.btnagfam.Text = "Agregar";
            this.btnagfam.UseVisualStyleBackColor = false;
            this.btnagfam.Click += new System.EventHandler(this.btnagfam_Click);
            // 
            // elimpermfam
            // 
            this.elimpermfam.BackColor = System.Drawing.Color.RosyBrown;
            this.elimpermfam.FlatAppearance.BorderSize = 0;
            this.elimpermfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.elimpermfam.Location = new System.Drawing.Point(622, 232);
            this.elimpermfam.Name = "elimpermfam";
            this.elimpermfam.Size = new System.Drawing.Size(150, 23);
            this.elimpermfam.TabIndex = 42;
            this.elimpermfam.Text = "Quitar";
            this.elimpermfam.UseVisualStyleBackColor = false;
            this.elimpermfam.Click += new System.EventHandler(this.elimpermfam_Click);
            // 
            // agpermfam
            // 
            this.agpermfam.BackColor = System.Drawing.Color.RosyBrown;
            this.agpermfam.FlatAppearance.BorderSize = 0;
            this.agpermfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.agpermfam.Location = new System.Drawing.Point(619, 203);
            this.agpermfam.Name = "agpermfam";
            this.agpermfam.Size = new System.Drawing.Size(169, 23);
            this.agpermfam.TabIndex = 41;
            this.agpermfam.Text = "Agregar";
            this.agpermfam.UseVisualStyleBackColor = false;
            this.agpermfam.Click += new System.EventHandler(this.agpermfam_Click);
            // 
            // btnelimfamfam
            // 
            this.btnelimfamfam.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelimfamfam.FlatAppearance.BorderSize = 0;
            this.btnelimfamfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelimfamfam.Location = new System.Drawing.Point(713, 356);
            this.btnelimfamfam.Name = "btnelimfamfam";
            this.btnelimfamfam.Size = new System.Drawing.Size(75, 23);
            this.btnelimfamfam.TabIndex = 44;
            this.btnelimfamfam.Text = "Quitar";
            this.btnelimfamfam.UseVisualStyleBackColor = false;
            this.btnelimfamfam.Click += new System.EventHandler(this.btnelimfamfam_Click);
            // 
            // btnagfamfam
            // 
            this.btnagfamfam.BackColor = System.Drawing.Color.RosyBrown;
            this.btnagfamfam.FlatAppearance.BorderSize = 0;
            this.btnagfamfam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnagfamfam.Location = new System.Drawing.Point(619, 356);
            this.btnagfamfam.Name = "btnagfamfam";
            this.btnagfamfam.Size = new System.Drawing.Size(75, 23);
            this.btnagfamfam.TabIndex = 43;
            this.btnagfamfam.Text = "Agregar";
            this.btnagfamfam.UseVisualStyleBackColor = false;
            this.btnagfamfam.Click += new System.EventHandler(this.btnagfamfam_Click);
            // 
            // btnelimpermperf
            // 
            this.btnelimpermperf.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelimpermperf.FlatAppearance.BorderSize = 0;
            this.btnelimpermperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelimpermperf.Location = new System.Drawing.Point(12, 232);
            this.btnelimpermperf.Name = "btnelimpermperf";
            this.btnelimpermperf.Size = new System.Drawing.Size(156, 23);
            this.btnelimpermperf.TabIndex = 46;
            this.btnelimpermperf.Text = "Quitar";
            this.btnelimpermperf.UseVisualStyleBackColor = false;
            this.btnelimpermperf.Click += new System.EventHandler(this.btnelimpermperf_Click);
            // 
            // btnagpermperf
            // 
            this.btnagpermperf.BackColor = System.Drawing.Color.RosyBrown;
            this.btnagpermperf.FlatAppearance.BorderSize = 0;
            this.btnagpermperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnagpermperf.Location = new System.Drawing.Point(12, 203);
            this.btnagpermperf.Name = "btnagpermperf";
            this.btnagpermperf.Size = new System.Drawing.Size(152, 23);
            this.btnagpermperf.TabIndex = 45;
            this.btnagpermperf.Text = "Agregar";
            this.btnagpermperf.UseVisualStyleBackColor = false;
            this.btnagpermperf.Click += new System.EventHandler(this.btnagpermperf_Click);
            // 
            // btnagfamperf
            // 
            this.btnagfamperf.BackColor = System.Drawing.Color.RosyBrown;
            this.btnagfamperf.FlatAppearance.BorderSize = 0;
            this.btnagfamperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnagfamperf.Location = new System.Drawing.Point(15, 338);
            this.btnagfamperf.Name = "btnagfamperf";
            this.btnagfamperf.Size = new System.Drawing.Size(152, 23);
            this.btnagfamperf.TabIndex = 47;
            this.btnagfamperf.Text = "Agregar";
            this.btnagfamperf.UseVisualStyleBackColor = false;
            this.btnagfamperf.Click += new System.EventHandler(this.btnagfamperf_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RosyBrown;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(245, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 36);
            this.button1.TabIndex = 52;
            this.button1.Text = "Limpiar Campos";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(242, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 50;
            this.label10.Text = "Perfiles Creados";
            // 
            // cmbperf
            // 
            this.cmbperf.FormattingEnabled = true;
            this.cmbperf.Location = new System.Drawing.Point(245, 89);
            this.cmbperf.Name = "cmbperf";
            this.cmbperf.Size = new System.Drawing.Size(121, 21);
            this.cmbperf.TabIndex = 53;
            this.cmbperf.SelectedIndexChanged += new System.EventHandler(this.cmbperf_SelectedIndexChanged);
            // 
            // cmbfam
            // 
            this.cmbfam.FormattingEnabled = true;
            this.cmbfam.Location = new System.Drawing.Point(443, 89);
            this.cmbfam.Name = "cmbfam";
            this.cmbfam.Size = new System.Drawing.Size(121, 21);
            this.cmbfam.TabIndex = 56;
            this.cmbfam.SelectedIndexChanged += new System.EventHandler(this.cmbfam_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(440, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 54;
            this.label11.Text = "Familias creadas";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.RosyBrown;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(224, 142);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(173, 23);
            this.button4.TabIndex = 57;
            this.button4.Text = "Limpiar Campos";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.RosyBrown;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(15, 367);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(152, 23);
            this.button2.TabIndex = 58;
            this.button2.Text = "Eliminar";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnmodperf
            // 
            this.btnmodperf.BackColor = System.Drawing.Color.RosyBrown;
            this.btnmodperf.FlatAppearance.BorderSize = 0;
            this.btnmodperf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmodperf.Location = new System.Drawing.Point(16, 87);
            this.btnmodperf.Name = "btnmodperf";
            this.btnmodperf.Size = new System.Drawing.Size(92, 23);
            this.btnmodperf.TabIndex = 59;
            this.btnmodperf.Text = "Modificar";
            this.btnmodperf.UseVisualStyleBackColor = false;
            this.btnmodperf.Click += new System.EventHandler(this.btnmodperf_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.RosyBrown;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(584, 89);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 23);
            this.button3.TabIndex = 60;
            this.button3.Text = "Modificar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormCrearPerfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnmodperf);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.cmbfam);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbperf);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnagfamperf);
            this.Controls.Add(this.btnelimpermperf);
            this.Controls.Add(this.btnagpermperf);
            this.Controls.Add(this.btnelimfamfam);
            this.Controls.Add(this.btnagfamfam);
            this.Controls.Add(this.elimpermfam);
            this.Controls.Add(this.agpermfam);
            this.Controls.Add(this.btnelimfam);
            this.Controls.Add(this.btnagfam);
            this.Controls.Add(this.btnelimperf);
            this.Controls.Add(this.agperf);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbfamfam);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtnomperf);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtfam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.cmbpermfam);
            this.Controls.Add(this.cmbpermperf);
            this.Controls.Add(this.cmbfamperf);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView2);
            this.Name = "FormCrearPerfiles";
            this.Text = "FormCrearPerfiles_750VR";
            this.Load += new System.EventHandler(this.FormCrearPerfiles_750VR_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbfamperf;
        private System.Windows.Forms.ComboBox cmbpermperf;
        private System.Windows.Forms.ComboBox cmbpermfam;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtfam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtnomperf;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbfamfam;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button agperf;
        private System.Windows.Forms.Button btnelimperf;
        private System.Windows.Forms.Button btnelimfam;
        private System.Windows.Forms.Button btnagfam;
        private System.Windows.Forms.Button elimpermfam;
        private System.Windows.Forms.Button agpermfam;
        private System.Windows.Forms.Button btnelimfamfam;
        private System.Windows.Forms.Button btnagfamfam;
        private System.Windows.Forms.Button btnelimpermperf;
        private System.Windows.Forms.Button btnagpermperf;
        private System.Windows.Forms.Button btnagfamperf;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbperf;
        private System.Windows.Forms.ComboBox cmbfam;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnmodperf;
        private System.Windows.Forms.Button button3;
    }
}