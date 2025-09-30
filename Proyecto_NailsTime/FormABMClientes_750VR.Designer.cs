namespace Proyecto_NailsTime
{
    partial class FormABMClientes
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtnom = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txtape = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtdni = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnañadir = new System.Windows.Forms.Button();
            this.btncance = new System.Windows.Forms.Button();
            this.btnsalir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtcel = new System.Windows.Forms.TextBox();
            this.txtdire = new System.Windows.Forms.TextBox();
            this.btnmod = new System.Windows.Forms.Button();
            this.btnelim = new System.Windows.Forms.Button();
            this.btnapli = new System.Windows.Forms.Button();
            this.lblmensaje = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.rbnActivos = new System.Windows.Forms.RadioButton();
            this.rbnTodos = new System.Windows.Forms.RadioButton();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.txtRutaImport = new System.Windows.Forms.TextBox();
            this.txtRutaExport = new System.Windows.Forms.TextBox();
            this.btnlimpiar = new System.Windows.Forms.Button();
            this.btnactualizar = new System.Windows.Forms.Button();
            this.btnBuscarImport = new FontAwesome.Sharp.IconButton();
            this.btnBuscarExport = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(615, 180);
            this.dataGridView1.TabIndex = 68;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 341);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 62;
            this.label7.Text = "Email";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 315);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "Nombre";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Apellido";
            // 
            // txtnom
            // 
            this.txtnom.Location = new System.Drawing.Point(68, 308);
            this.txtnom.Name = "txtnom";
            this.txtnom.Size = new System.Drawing.Size(100, 20);
            this.txtnom.TabIndex = 48;
            this.txtnom.TextChanged += new System.EventHandler(this.txtnom_TextChanged);
            // 
            // txtemail
            // 
            this.txtemail.Location = new System.Drawing.Point(68, 334);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(100, 20);
            this.txtemail.TabIndex = 49;
            this.txtemail.TextChanged += new System.EventHandler(this.txtemail_TextChanged);
            // 
            // txtape
            // 
            this.txtape.Location = new System.Drawing.Point(68, 282);
            this.txtape.Name = "txtape";
            this.txtape.Size = new System.Drawing.Size(100, 20);
            this.txtape.TabIndex = 47;
            this.txtape.TextChanged += new System.EventHandler(this.txtape_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "DNI";
            // 
            // txtdni
            // 
            this.txtdni.Location = new System.Drawing.Point(68, 254);
            this.txtdni.Name = "txtdni";
            this.txtdni.Size = new System.Drawing.Size(100, 20);
            this.txtdni.TabIndex = 46;
            this.txtdni.TextChanged += new System.EventHandler(this.txtdni_TextChanged);
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 45);
            this.label2.TabIndex = 53;
            this.label2.Text = "ABM clientes";
            // 
            // btnañadir
            // 
            this.btnañadir.BackColor = System.Drawing.Color.RosyBrown;
            this.btnañadir.FlatAppearance.BorderSize = 0;
            this.btnañadir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnañadir.Location = new System.Drawing.Point(256, 310);
            this.btnañadir.Name = "btnañadir";
            this.btnañadir.Size = new System.Drawing.Size(75, 23);
            this.btnañadir.TabIndex = 69;
            this.btnañadir.Text = "Añadir";
            this.btnañadir.UseVisualStyleBackColor = false;
            this.btnañadir.Click += new System.EventHandler(this.btnañadir_Click);
            // 
            // btncance
            // 
            this.btncance.BackColor = System.Drawing.Color.RosyBrown;
            this.btncance.FlatAppearance.BorderSize = 0;
            this.btncance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncance.Location = new System.Drawing.Point(362, 341);
            this.btncance.Name = "btncance";
            this.btncance.Size = new System.Drawing.Size(75, 23);
            this.btncance.TabIndex = 70;
            this.btncance.Text = "Cancelar";
            this.btncance.UseVisualStyleBackColor = false;
            this.btncance.Click += new System.EventHandler(this.btncance_Click);
            // 
            // btnsalir
            // 
            this.btnsalir.BackColor = System.Drawing.Color.RosyBrown;
            this.btnsalir.FlatAppearance.BorderSize = 0;
            this.btnsalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsalir.Location = new System.Drawing.Point(362, 372);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(75, 23);
            this.btnsalir.TabIndex = 71;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = false;
            this.btnsalir.Click += new System.EventHandler(this.btnsalir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 75;
            this.label1.Text = "Direccion";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 367);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 74;
            this.label3.Text = "Celular";
            // 
            // txtcel
            // 
            this.txtcel.Location = new System.Drawing.Point(68, 360);
            this.txtcel.Name = "txtcel";
            this.txtcel.Size = new System.Drawing.Size(100, 20);
            this.txtcel.TabIndex = 72;
            this.txtcel.TextChanged += new System.EventHandler(this.txtcel_TextChanged);
            // 
            // txtdire
            // 
            this.txtdire.Location = new System.Drawing.Point(68, 386);
            this.txtdire.Name = "txtdire";
            this.txtdire.Size = new System.Drawing.Size(100, 20);
            this.txtdire.TabIndex = 73;
            this.txtdire.TextChanged += new System.EventHandler(this.txtdire_TextChanged);
            // 
            // btnmod
            // 
            this.btnmod.BackColor = System.Drawing.Color.RosyBrown;
            this.btnmod.FlatAppearance.BorderSize = 0;
            this.btnmod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmod.Location = new System.Drawing.Point(256, 341);
            this.btnmod.Name = "btnmod";
            this.btnmod.Size = new System.Drawing.Size(75, 23);
            this.btnmod.TabIndex = 76;
            this.btnmod.Text = "Modificar";
            this.btnmod.UseVisualStyleBackColor = false;
            this.btnmod.Click += new System.EventHandler(this.btnmod_Click);
            // 
            // btnelim
            // 
            this.btnelim.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelim.FlatAppearance.BorderSize = 0;
            this.btnelim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelim.Location = new System.Drawing.Point(256, 372);
            this.btnelim.Name = "btnelim";
            this.btnelim.Size = new System.Drawing.Size(75, 23);
            this.btnelim.TabIndex = 77;
            this.btnelim.Text = "Eliminar";
            this.btnelim.UseVisualStyleBackColor = false;
            this.btnelim.Click += new System.EventHandler(this.btnelim_Click);
            // 
            // btnapli
            // 
            this.btnapli.BackColor = System.Drawing.Color.RosyBrown;
            this.btnapli.FlatAppearance.BorderSize = 0;
            this.btnapli.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnapli.Location = new System.Drawing.Point(362, 310);
            this.btnapli.Name = "btnapli";
            this.btnapli.Size = new System.Drawing.Size(75, 23);
            this.btnapli.TabIndex = 78;
            this.btnapli.Text = "Aplicar";
            this.btnapli.UseVisualStyleBackColor = false;
            this.btnapli.Click += new System.EventHandler(this.btnapli_Click);
            // 
            // lblmensaje
            // 
            this.lblmensaje.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblmensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmensaje.Location = new System.Drawing.Point(263, 235);
            this.lblmensaje.Name = "lblmensaje";
            this.lblmensaje.Size = new System.Drawing.Size(371, 45);
            this.lblmensaje.TabIndex = 79;
            this.lblmensaje.Click += new System.EventHandler(this.lblmensaje_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(185, 337);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(41, 17);
            this.checkBox1.TabIndex = 80;
            this.checkBox1.Text = "ver";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // rbnActivos
            // 
            this.rbnActivos.AutoSize = true;
            this.rbnActivos.Location = new System.Drawing.Point(426, 25);
            this.rbnActivos.Name = "rbnActivos";
            this.rbnActivos.Size = new System.Drawing.Size(60, 17);
            this.rbnActivos.TabIndex = 81;
            this.rbnActivos.TabStop = true;
            this.rbnActivos.Text = "Activos";
            this.rbnActivos.UseVisualStyleBackColor = true;
            this.rbnActivos.CheckedChanged += new System.EventHandler(this.rbnActivos_CheckedChanged);
            // 
            // rbnTodos
            // 
            this.rbnTodos.AutoSize = true;
            this.rbnTodos.Location = new System.Drawing.Point(505, 25);
            this.rbnTodos.Name = "rbnTodos";
            this.rbnTodos.Size = new System.Drawing.Size(51, 17);
            this.rbnTodos.TabIndex = 82;
            this.rbnTodos.TabStop = true;
            this.rbnTodos.Text = "todos";
            this.rbnTodos.UseVisualStyleBackColor = true;
            this.rbnTodos.CheckedChanged += new System.EventHandler(this.rbnTodos_CheckedChanged);
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(571, 372);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(75, 23);
            this.btnImportar.TabIndex = 88;
            this.btnImportar.Text = "Des-serializar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click_1);
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(571, 317);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(75, 23);
            this.btnExportar.TabIndex = 87;
            this.btnExportar.Text = "Serializar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click_1);
            // 
            // txtRutaImport
            // 
            this.txtRutaImport.Location = new System.Drawing.Point(545, 346);
            this.txtRutaImport.Name = "txtRutaImport";
            this.txtRutaImport.Size = new System.Drawing.Size(126, 20);
            this.txtRutaImport.TabIndex = 86;
            // 
            // txtRutaExport
            // 
            this.txtRutaExport.Location = new System.Drawing.Point(545, 289);
            this.txtRutaExport.Name = "txtRutaExport";
            this.txtRutaExport.Size = new System.Drawing.Size(126, 20);
            this.txtRutaExport.TabIndex = 85;
            this.txtRutaExport.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnlimpiar
            // 
            this.btnlimpiar.BackColor = System.Drawing.Color.RosyBrown;
            this.btnlimpiar.FlatAppearance.BorderSize = 0;
            this.btnlimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlimpiar.Location = new System.Drawing.Point(649, 245);
            this.btnlimpiar.Name = "btnlimpiar";
            this.btnlimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnlimpiar.TabIndex = 91;
            this.btnlimpiar.Text = "Limpiar";
            this.btnlimpiar.UseVisualStyleBackColor = false;
            this.btnlimpiar.Click += new System.EventHandler(this.btnlimpiar_Click);
            // 
            // btnactualizar
            // 
            this.btnactualizar.BackColor = System.Drawing.Color.RosyBrown;
            this.btnactualizar.FlatAppearance.BorderSize = 0;
            this.btnactualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnactualizar.Location = new System.Drawing.Point(649, 209);
            this.btnactualizar.Name = "btnactualizar";
            this.btnactualizar.Size = new System.Drawing.Size(75, 23);
            this.btnactualizar.TabIndex = 92;
            this.btnactualizar.Text = "Actualizar";
            this.btnactualizar.UseVisualStyleBackColor = false;
            this.btnactualizar.Click += new System.EventHandler(this.btnactualizar_Click);
            // 
            // btnBuscarImport
            // 
            this.btnBuscarImport.IconChar = FontAwesome.Sharp.IconChar.Folder;
            this.btnBuscarImport.IconColor = System.Drawing.Color.Black;
            this.btnBuscarImport.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscarImport.Location = new System.Drawing.Point(677, 341);
            this.btnBuscarImport.Name = "btnBuscarImport";
            this.btnBuscarImport.Size = new System.Drawing.Size(57, 54);
            this.btnBuscarImport.TabIndex = 89;
            this.btnBuscarImport.UseVisualStyleBackColor = true;
            this.btnBuscarImport.Click += new System.EventHandler(this.btnBuscarImport_Click_1);
            // 
            // btnBuscarExport
            // 
            this.btnBuscarExport.IconChar = FontAwesome.Sharp.IconChar.Folder;
            this.btnBuscarExport.IconColor = System.Drawing.Color.Black;
            this.btnBuscarExport.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscarExport.Location = new System.Drawing.Point(677, 282);
            this.btnBuscarExport.Name = "btnBuscarExport";
            this.btnBuscarExport.Size = new System.Drawing.Size(57, 53);
            this.btnBuscarExport.TabIndex = 90;
            this.btnBuscarExport.UseVisualStyleBackColor = true;
            this.btnBuscarExport.Click += new System.EventHandler(this.btnBuscarExport_Click_1);
            // 
            // FormABMClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(750, 410);
            this.Controls.Add(this.btnactualizar);
            this.Controls.Add(this.btnlimpiar);
            this.Controls.Add(this.btnBuscarImport);
            this.Controls.Add(this.btnBuscarExport);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.txtRutaImport);
            this.Controls.Add(this.txtRutaExport);
            this.Controls.Add(this.rbnTodos);
            this.Controls.Add(this.rbnActivos);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lblmensaje);
            this.Controls.Add(this.btnapli);
            this.Controls.Add(this.btnelim);
            this.Controls.Add(this.btnmod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtcel);
            this.Controls.Add(this.txtdire);
            this.Controls.Add(this.btnsalir);
            this.Controls.Add(this.btncance);
            this.Controls.Add(this.btnañadir);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtnom);
            this.Controls.Add(this.txtemail);
            this.Controls.Add(this.txtape);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtdni);
            this.Controls.Add(this.label2);
            this.Name = "FormABMClientes";
            this.Text = "FormRegistroClientes";
            this.Load += new System.EventHandler(this.FormABMClientes_750VR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtnom;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txtape;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtdni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnañadir;
        private System.Windows.Forms.Button btncance;
        private System.Windows.Forms.Button btnsalir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtcel;
        private System.Windows.Forms.TextBox txtdire;
        private System.Windows.Forms.Button btnmod;
        private System.Windows.Forms.Button btnelim;
        private System.Windows.Forms.Button btnapli;
        private System.Windows.Forms.Label lblmensaje;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton rbnActivos;
        private System.Windows.Forms.RadioButton rbnTodos;
        private FontAwesome.Sharp.IconButton btnBuscarImport;
        private FontAwesome.Sharp.IconButton btnBuscarExport;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.TextBox txtRutaImport;
        private System.Windows.Forms.TextBox txtRutaExport;
        private System.Windows.Forms.Button btnlimpiar;
        private System.Windows.Forms.Button btnactualizar;
    }
}