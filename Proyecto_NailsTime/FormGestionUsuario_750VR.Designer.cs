namespace Proyecto_NailsTime
{
    partial class FormGestionUsuario_750VR
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
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.rbtnact = new System.Windows.Forms.RadioButton();
            this.rbtntodos = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblcantuser = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtape = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txtnom = new System.Windows.Forms.TextBox();
            this.cmbrol = new System.Windows.Forms.ComboBox();
            this.actsi = new System.Windows.Forms.RadioButton();
            this.actno = new System.Windows.Forms.RadioButton();
            this.bloqsi = new System.Windows.Forms.RadioButton();
            this.bloqno = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btncrear = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btndesb = new System.Windows.Forms.Button();
            this.btnmod = new System.Windows.Forms.Button();
            this.btnact = new System.Windows.Forms.Button();
            this.btnaplicar = new System.Windows.Forms.Button();
            this.btncancelar = new System.Windows.Forms.Button();
            this.btnsalir = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblmensaje = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnelim = new System.Windows.Forms.Button();
            this.txtuser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 45);
            this.label2.TabIndex = 7;
            this.label2.Text = "Gestion de usuarios";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(18, 69);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(538, 150);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // rbtnact
            // 
            this.rbtnact.AutoSize = true;
            this.rbtnact.Location = new System.Drawing.Point(340, 34);
            this.rbtnact.Name = "rbtnact";
            this.rbtnact.Size = new System.Drawing.Size(60, 17);
            this.rbtnact.TabIndex = 9;
            this.rbtnact.TabStop = true;
            this.rbtnact.Text = "Activos";
            this.rbtnact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbtnact.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.rbtnact.UseVisualStyleBackColor = true;
            this.rbtnact.CheckedChanged += new System.EventHandler(this.rbtnact_CheckedChanged);
            // 
            // rbtntodos
            // 
            this.rbtntodos.AutoSize = true;
            this.rbtntodos.Location = new System.Drawing.Point(429, 34);
            this.rbtntodos.Name = "rbtntodos";
            this.rbtntodos.Size = new System.Drawing.Size(55, 17);
            this.rbtntodos.TabIndex = 10;
            this.rbtntodos.TabStop = true;
            this.rbtntodos.Text = "Todos";
            this.rbtntodos.UseVisualStyleBackColor = true;
            this.rbtntodos.CheckedChanged += new System.EventHandler(this.rbtntodos_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(554, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 30);
            this.label1.TabIndex = 11;
            this.label1.Text = "Numero de usuarios:";
            // 
            // lblcantuser
            // 
            this.lblcantuser.AutoSize = true;
            this.lblcantuser.Location = new System.Drawing.Point(721, 26);
            this.lblcantuser.Name = "lblcantuser";
            this.lblcantuser.Size = new System.Drawing.Size(0, 13);
            this.lblcantuser.TabIndex = 12;
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(61, 284);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(100, 20);
            this.txtDNI.TabIndex = 13;
            this.txtDNI.TextChanged += new System.EventHandler(this.txtDNI_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "DNI";
            // 
            // txtape
            // 
            this.txtape.Location = new System.Drawing.Point(61, 324);
            this.txtape.Name = "txtape";
            this.txtape.Size = new System.Drawing.Size(100, 20);
            this.txtape.TabIndex = 15;
            this.txtape.TextChanged += new System.EventHandler(this.txtape_TextChanged);
            // 
            // txtemail
            // 
            this.txtemail.Location = new System.Drawing.Point(61, 418);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(100, 20);
            this.txtemail.TabIndex = 16;
            this.txtemail.TextChanged += new System.EventHandler(this.txtemail_TextChanged);
            // 
            // txtnom
            // 
            this.txtnom.Location = new System.Drawing.Point(61, 371);
            this.txtnom.Name = "txtnom";
            this.txtnom.Size = new System.Drawing.Size(100, 20);
            this.txtnom.TabIndex = 17;
            this.txtnom.TextChanged += new System.EventHandler(this.txtnom_TextChanged);
            // 
            // cmbrol
            // 
            this.cmbrol.FormattingEnabled = true;
            this.cmbrol.Location = new System.Drawing.Point(262, 283);
            this.cmbrol.Name = "cmbrol";
            this.cmbrol.Size = new System.Drawing.Size(121, 21);
            this.cmbrol.TabIndex = 19;
            this.cmbrol.TextChanged += new System.EventHandler(this.cmbrol_TextChanged);
            // 
            // actsi
            // 
            this.actsi.AutoSize = true;
            this.actsi.Location = new System.Drawing.Point(262, 378);
            this.actsi.Name = "actsi";
            this.actsi.Size = new System.Drawing.Size(35, 17);
            this.actsi.TabIndex = 20;
            this.actsi.TabStop = true;
            this.actsi.Text = "SI";
            this.actsi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.actsi.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.actsi.UseVisualStyleBackColor = true;
            // 
            // actno
            // 
            this.actno.AutoSize = true;
            this.actno.Location = new System.Drawing.Point(303, 378);
            this.actno.Name = "actno";
            this.actno.Size = new System.Drawing.Size(41, 17);
            this.actno.TabIndex = 21;
            this.actno.TabStop = true;
            this.actno.Text = "NO";
            this.actno.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.actno.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.actno.UseVisualStyleBackColor = true;
            // 
            // bloqsi
            // 
            this.bloqsi.AutoSize = true;
            this.bloqsi.Location = new System.Drawing.Point(262, 420);
            this.bloqsi.Name = "bloqsi";
            this.bloqsi.Size = new System.Drawing.Size(35, 17);
            this.bloqsi.TabIndex = 22;
            this.bloqsi.TabStop = true;
            this.bloqsi.Text = "SI";
            this.bloqsi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bloqsi.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.bloqsi.UseVisualStyleBackColor = true;
            // 
            // bloqno
            // 
            this.bloqno.AutoSize = true;
            this.bloqno.Location = new System.Drawing.Point(303, 420);
            this.bloqno.Name = "bloqno";
            this.bloqno.Size = new System.Drawing.Size(41, 17);
            this.bloqno.TabIndex = 23;
            this.bloqno.TabStop = true;
            this.bloqno.Text = "NO";
            this.bloqno.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bloqno.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.bloqno.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 331);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Apellido";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 378);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Nombre";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 425);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Email";
            // 
            // btncrear
            // 
            this.btncrear.Location = new System.Drawing.Point(562, 69);
            this.btncrear.Name = "btncrear";
            this.btncrear.Size = new System.Drawing.Size(75, 23);
            this.btncrear.TabIndex = 27;
            this.btncrear.Text = "Crear";
            this.btncrear.UseVisualStyleBackColor = true;
            this.btncrear.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(193, 287);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Rol";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(193, 378);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Activo";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(193, 422);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Bloqueado";
            // 
            // btndesb
            // 
            this.btndesb.Location = new System.Drawing.Point(651, 69);
            this.btndesb.Name = "btndesb";
            this.btndesb.Size = new System.Drawing.Size(75, 23);
            this.btndesb.TabIndex = 32;
            this.btndesb.Text = "Desbloquear";
            this.btndesb.UseVisualStyleBackColor = true;
            this.btndesb.Click += new System.EventHandler(this.btndesb_Click);
            // 
            // btnmod
            // 
            this.btnmod.Location = new System.Drawing.Point(651, 117);
            this.btnmod.Name = "btnmod";
            this.btnmod.Size = new System.Drawing.Size(75, 23);
            this.btnmod.TabIndex = 33;
            this.btnmod.Text = "Modificar";
            this.btnmod.UseVisualStyleBackColor = true;
            // 
            // btnact
            // 
            this.btnact.Location = new System.Drawing.Point(651, 277);
            this.btnact.Name = "btnact";
            this.btnact.Size = new System.Drawing.Size(75, 23);
            this.btnact.TabIndex = 34;
            this.btnact.Text = "Act/Desact";
            this.btnact.UseVisualStyleBackColor = true;
            // 
            // btnaplicar
            // 
            this.btnaplicar.Location = new System.Drawing.Point(603, 165);
            this.btnaplicar.Name = "btnaplicar";
            this.btnaplicar.Size = new System.Drawing.Size(75, 23);
            this.btnaplicar.TabIndex = 35;
            this.btnaplicar.Text = "Aplicar";
            this.btnaplicar.UseVisualStyleBackColor = true;
            this.btnaplicar.Click += new System.EventHandler(this.btnaplicar_Click);
            // 
            // btncancelar
            // 
            this.btncancelar.Location = new System.Drawing.Point(651, 322);
            this.btncancelar.Name = "btncancelar";
            this.btncancelar.Size = new System.Drawing.Size(75, 23);
            this.btncancelar.TabIndex = 36;
            this.btncancelar.Text = "Cancelar";
            this.btncancelar.UseVisualStyleBackColor = true;
            this.btncancelar.Click += new System.EventHandler(this.btncancelar_Click);
            // 
            // btnsalir
            // 
            this.btnsalir.Location = new System.Drawing.Point(651, 372);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(75, 23);
            this.btnsalir.TabIndex = 37;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = true;
            this.btnsalir.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblmensaje);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Location = new System.Drawing.Point(416, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 175);
            this.panel1.TabIndex = 38;
            // 
            // lblmensaje
            // 
            this.lblmensaje.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblmensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmensaje.Location = new System.Drawing.Point(20, 50);
            this.lblmensaje.Name = "lblmensaje";
            this.lblmensaje.Size = new System.Drawing.Size(161, 30);
            this.lblmensaje.TabIndex = 42;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(33, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Mensaje:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // btnelim
            // 
            this.btnelim.Location = new System.Drawing.Point(562, 117);
            this.btnelim.Name = "btnelim";
            this.btnelim.Size = new System.Drawing.Size(75, 23);
            this.btnelim.TabIndex = 39;
            this.btnelim.Text = "Eliminar";
            this.btnelim.UseVisualStyleBackColor = true;
            this.btnelim.Click += new System.EventHandler(this.btnelim_Click);
            // 
            // txtuser
            // 
            this.txtuser.Location = new System.Drawing.Point(262, 328);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(100, 20);
            this.txtuser.TabIndex = 40;
            this.txtuser.TextChanged += new System.EventHandler(this.txtuser_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(193, 335);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "User";
            // 
            // FormGestionUsuario_750VR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtuser);
            this.Controls.Add(this.btnelim);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnsalir);
            this.Controls.Add(this.btncancelar);
            this.Controls.Add(this.btnaplicar);
            this.Controls.Add(this.btnact);
            this.Controls.Add(this.btnmod);
            this.Controls.Add(this.btndesb);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btncrear);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bloqno);
            this.Controls.Add(this.bloqsi);
            this.Controls.Add(this.actno);
            this.Controls.Add(this.actsi);
            this.Controls.Add(this.cmbrol);
            this.Controls.Add(this.txtnom);
            this.Controls.Add(this.txtemail);
            this.Controls.Add(this.txtape);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblcantuser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbtntodos);
            this.Controls.Add(this.rbtnact);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Name = "FormGestionUsuario_750VR";
            this.Text = "FormAgregarUsuario_750VR";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton rbtnact;
        private System.Windows.Forms.RadioButton rbtntodos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblcantuser;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtape;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txtnom;
        private System.Windows.Forms.ComboBox cmbrol;
        private System.Windows.Forms.RadioButton actsi;
        private System.Windows.Forms.RadioButton actno;
        private System.Windows.Forms.RadioButton bloqsi;
        private System.Windows.Forms.RadioButton bloqno;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btncrear;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btndesb;
        private System.Windows.Forms.Button btnmod;
        private System.Windows.Forms.Button btnact;
        private System.Windows.Forms.Button btnaplicar;
        private System.Windows.Forms.Button btncancelar;
        private System.Windows.Forms.Button btnsalir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnelim;
        private System.Windows.Forms.TextBox txtuser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblmensaje;
    }
}