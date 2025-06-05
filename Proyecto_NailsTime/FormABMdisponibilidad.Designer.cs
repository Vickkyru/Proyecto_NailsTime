namespace Proyecto_NailsTime
{
    partial class FormABMdisponibilidad
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
            this.lblmensaje = new System.Windows.Forms.Label();
            this.btnapli = new System.Windows.Forms.Button();
            this.btnelim = new System.Windows.Forms.Button();
            this.btnmod = new System.Windows.Forms.Button();
            this.btnsalir = new System.Windows.Forms.Button();
            this.btncance = new System.Windows.Forms.Button();
            this.btnañadir = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Fecha = new System.Windows.Forms.Label();
            this.txtinicio = new System.Windows.Forms.TextBox();
            this.txtfin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtdnimanic = new System.Windows.Forms.TextBox();
            this.cmbmanic = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblmensaje
            // 
            this.lblmensaje.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblmensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmensaje.Location = new System.Drawing.Point(360, 274);
            this.lblmensaje.Name = "lblmensaje";
            this.lblmensaje.Size = new System.Drawing.Size(371, 45);
            this.lblmensaje.TabIndex = 100;
            this.lblmensaje.Click += new System.EventHandler(this.lblmensaje_Click);
            // 
            // btnapli
            // 
            this.btnapli.Location = new System.Drawing.Point(509, 330);
            this.btnapli.Name = "btnapli";
            this.btnapli.Size = new System.Drawing.Size(75, 23);
            this.btnapli.TabIndex = 99;
            this.btnapli.Text = "Aplicar";
            this.btnapli.UseVisualStyleBackColor = true;
            this.btnapli.Click += new System.EventHandler(this.btnapli_Click);
            // 
            // btnelim
            // 
            this.btnelim.Location = new System.Drawing.Point(403, 392);
            this.btnelim.Name = "btnelim";
            this.btnelim.Size = new System.Drawing.Size(75, 23);
            this.btnelim.TabIndex = 98;
            this.btnelim.Text = "Eliminar";
            this.btnelim.UseVisualStyleBackColor = true;
            this.btnelim.Click += new System.EventHandler(this.btnelim_Click);
            // 
            // btnmod
            // 
            this.btnmod.Location = new System.Drawing.Point(403, 361);
            this.btnmod.Name = "btnmod";
            this.btnmod.Size = new System.Drawing.Size(75, 23);
            this.btnmod.TabIndex = 97;
            this.btnmod.Text = "Modificar";
            this.btnmod.UseVisualStyleBackColor = true;
            this.btnmod.Click += new System.EventHandler(this.btnmod_Click);
            // 
            // btnsalir
            // 
            this.btnsalir.Location = new System.Drawing.Point(509, 392);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(75, 23);
            this.btnsalir.TabIndex = 92;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = true;
            this.btnsalir.Click += new System.EventHandler(this.btnsalir_Click);
            // 
            // btncance
            // 
            this.btncance.Location = new System.Drawing.Point(509, 361);
            this.btncance.Name = "btncance";
            this.btncance.Size = new System.Drawing.Size(75, 23);
            this.btncance.TabIndex = 91;
            this.btncance.Text = "Cancelar";
            this.btncance.UseVisualStyleBackColor = true;
            this.btncance.Click += new System.EventHandler(this.btncance_Click);
            // 
            // btnañadir
            // 
            this.btnañadir.Location = new System.Drawing.Point(403, 330);
            this.btnañadir.Name = "btnañadir";
            this.btnañadir.Size = new System.Drawing.Size(75, 23);
            this.btnañadir.TabIndex = 90;
            this.btnañadir.Text = "Añadir";
            this.btnañadir.UseVisualStyleBackColor = true;
            this.btnañadir.Click += new System.EventHandler(this.btnañadir_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(702, 172);
            this.dataGridView1.TabIndex = 89;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(82, 389);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 88;
            this.label7.Text = "Hora Fin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 361);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 87;
            this.label6.Text = "Hora Inicio";
            // 
            // Fecha
            // 
            this.Fecha.AutoSize = true;
            this.Fecha.Location = new System.Drawing.Point(82, 334);
            this.Fecha.Name = "Fecha";
            this.Fecha.Size = new System.Drawing.Size(37, 13);
            this.Fecha.TabIndex = 86;
            this.Fecha.Text = "Fecha";
            // 
            // txtinicio
            // 
            this.txtinicio.Location = new System.Drawing.Point(156, 356);
            this.txtinicio.Name = "txtinicio";
            this.txtinicio.Size = new System.Drawing.Size(100, 20);
            this.txtinicio.TabIndex = 82;
            // 
            // txtfin
            // 
            this.txtfin.Location = new System.Drawing.Point(156, 382);
            this.txtfin.Name = "txtfin";
            this.txtfin.Size = new System.Drawing.Size(100, 20);
            this.txtfin.TabIndex = 83;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 306);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 85;
            this.label4.Text = "Manicurista";
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 45);
            this.label2.TabIndex = 84;
            this.label2.Text = "ABM disponibilidad";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 104;
            this.label1.Text = "DNI manicurista";
            // 
            // txtdnimanic
            // 
            this.txtdnimanic.Location = new System.Drawing.Point(156, 267);
            this.txtdnimanic.Name = "txtdnimanic";
            this.txtdnimanic.Size = new System.Drawing.Size(100, 20);
            this.txtdnimanic.TabIndex = 105;
            // 
            // cmbmanic
            // 
            this.cmbmanic.FormattingEnabled = true;
            this.cmbmanic.Location = new System.Drawing.Point(156, 298);
            this.cmbmanic.Name = "cmbmanic";
            this.cmbmanic.Size = new System.Drawing.Size(121, 21);
            this.cmbmanic.TabIndex = 106;
            this.cmbmanic.SelectedIndexChanged += new System.EventHandler(this.cmbmanic_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(156, 329);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 108;
            // 
            // FormABMdisponibilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.cmbmanic);
            this.Controls.Add(this.txtdnimanic);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblmensaje);
            this.Controls.Add(this.btnapli);
            this.Controls.Add(this.btnelim);
            this.Controls.Add(this.btnmod);
            this.Controls.Add(this.btnsalir);
            this.Controls.Add(this.btncance);
            this.Controls.Add(this.btnañadir);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Fecha);
            this.Controls.Add(this.txtinicio);
            this.Controls.Add(this.txtfin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "FormABMdisponibilidad";
            this.Text = "FormABMdisponibilidad";
            this.Load += new System.EventHandler(this.FormABMdisponibilidad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblmensaje;
        private System.Windows.Forms.Button btnapli;
        private System.Windows.Forms.Button btnelim;
        private System.Windows.Forms.Button btnmod;
        private System.Windows.Forms.Button btnsalir;
        private System.Windows.Forms.Button btncance;
        private System.Windows.Forms.Button btnañadir;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Fecha;
        private System.Windows.Forms.TextBox txtinicio;
        private System.Windows.Forms.TextBox txtfin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtdnimanic;
        private System.Windows.Forms.ComboBox cmbmanic;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}