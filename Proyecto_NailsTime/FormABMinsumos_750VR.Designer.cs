namespace Proyecto_NailsTime
{
    partial class FormABMinsumos_750VR
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
            this.rbnTodos = new System.Windows.Forms.RadioButton();
            this.rbnActivos = new System.Windows.Forms.RadioButton();
            this.txtdesc = new System.Windows.Forms.TextBox();
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
            this.label5 = new System.Windows.Forms.Label();
            this.txtcant = new System.Windows.Forms.TextBox();
            this.txtstock = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtunidad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // rbnTodos
            // 
            this.rbnTodos.AutoSize = true;
            this.rbnTodos.Location = new System.Drawing.Point(596, 31);
            this.rbnTodos.Name = "rbnTodos";
            this.rbnTodos.Size = new System.Drawing.Size(51, 17);
            this.rbnTodos.TabIndex = 122;
            this.rbnTodos.TabStop = true;
            this.rbnTodos.Text = "todos";
            this.rbnTodos.UseVisualStyleBackColor = true;
            this.rbnTodos.CheckedChanged += new System.EventHandler(this.rbnTodos_CheckedChanged);
            // 
            // rbnActivos
            // 
            this.rbnActivos.AutoSize = true;
            this.rbnActivos.Location = new System.Drawing.Point(517, 31);
            this.rbnActivos.Name = "rbnActivos";
            this.rbnActivos.Size = new System.Drawing.Size(60, 17);
            this.rbnActivos.TabIndex = 121;
            this.rbnActivos.TabStop = true;
            this.rbnActivos.Text = "Activos";
            this.rbnActivos.UseVisualStyleBackColor = true;
            this.rbnActivos.CheckedChanged += new System.EventHandler(this.rbnActivos_CheckedChanged);
            // 
            // txtdesc
            // 
            this.txtdesc.Location = new System.Drawing.Point(146, 334);
            this.txtdesc.Name = "txtdesc";
            this.txtdesc.Size = new System.Drawing.Size(121, 20);
            this.txtdesc.TabIndex = 120;
            // 
            // lblmensaje
            // 
            this.lblmensaje.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblmensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmensaje.Location = new System.Drawing.Point(333, 273);
            this.lblmensaje.Name = "lblmensaje";
            this.lblmensaje.Size = new System.Drawing.Size(371, 45);
            this.lblmensaje.TabIndex = 119;
            // 
            // btnapli
            // 
            this.btnapli.BackColor = System.Drawing.Color.RosyBrown;
            this.btnapli.FlatAppearance.BorderSize = 0;
            this.btnapli.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnapli.Location = new System.Drawing.Point(514, 332);
            this.btnapli.Name = "btnapli";
            this.btnapli.Size = new System.Drawing.Size(75, 23);
            this.btnapli.TabIndex = 118;
            this.btnapli.Text = "Aplicar";
            this.btnapli.UseVisualStyleBackColor = false;
            this.btnapli.Click += new System.EventHandler(this.btnapli_Click);
            // 
            // btnelim
            // 
            this.btnelim.BackColor = System.Drawing.Color.RosyBrown;
            this.btnelim.FlatAppearance.BorderSize = 0;
            this.btnelim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnelim.Location = new System.Drawing.Point(408, 394);
            this.btnelim.Name = "btnelim";
            this.btnelim.Size = new System.Drawing.Size(75, 23);
            this.btnelim.TabIndex = 117;
            this.btnelim.Text = "Eliminar";
            this.btnelim.UseVisualStyleBackColor = false;
            this.btnelim.Click += new System.EventHandler(this.btnelim_Click);
            // 
            // btnmod
            // 
            this.btnmod.BackColor = System.Drawing.Color.RosyBrown;
            this.btnmod.FlatAppearance.BorderSize = 0;
            this.btnmod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnmod.Location = new System.Drawing.Point(408, 363);
            this.btnmod.Name = "btnmod";
            this.btnmod.Size = new System.Drawing.Size(75, 23);
            this.btnmod.TabIndex = 116;
            this.btnmod.Text = "Modificar";
            this.btnmod.UseVisualStyleBackColor = false;
            this.btnmod.Click += new System.EventHandler(this.btnmod_Click);
            // 
            // btnsalir
            // 
            this.btnsalir.BackColor = System.Drawing.Color.RosyBrown;
            this.btnsalir.FlatAppearance.BorderSize = 0;
            this.btnsalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsalir.Location = new System.Drawing.Point(514, 394);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(75, 23);
            this.btnsalir.TabIndex = 115;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = false;
            this.btnsalir.Click += new System.EventHandler(this.btnsalir_Click);
            // 
            // btncance
            // 
            this.btncance.BackColor = System.Drawing.Color.RosyBrown;
            this.btncance.FlatAppearance.BorderSize = 0;
            this.btncance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncance.Location = new System.Drawing.Point(514, 363);
            this.btncance.Name = "btncance";
            this.btncance.Size = new System.Drawing.Size(75, 23);
            this.btncance.TabIndex = 114;
            this.btncance.Text = "Cancelar";
            this.btncance.UseVisualStyleBackColor = false;
            this.btncance.Click += new System.EventHandler(this.btncance_Click);
            // 
            // btnañadir
            // 
            this.btnañadir.BackColor = System.Drawing.Color.RosyBrown;
            this.btnañadir.FlatAppearance.BorderSize = 0;
            this.btnañadir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnañadir.Location = new System.Drawing.Point(408, 332);
            this.btnañadir.Name = "btnañadir";
            this.btnañadir.Size = new System.Drawing.Size(75, 23);
            this.btnañadir.TabIndex = 113;
            this.btnañadir.Text = "Añadir";
            this.btnañadir.UseVisualStyleBackColor = false;
            this.btnañadir.Click += new System.EventHandler(this.btnañadir_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(36, 79);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(641, 180);
            this.dataGridView1.TabIndex = 112;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(58, 369);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 111;
            this.label7.Text = "Cantidad";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 315);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 110;
            this.label6.Text = "Nombre";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(58, 342);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 109;
            this.label5.Text = "Descripcion";
            // 
            // txtcant
            // 
            this.txtcant.Location = new System.Drawing.Point(146, 362);
            this.txtcant.Name = "txtcant";
            this.txtcant.Size = new System.Drawing.Size(121, 20);
            this.txtcant.TabIndex = 105;
            // 
            // txtstock
            // 
            this.txtstock.Location = new System.Drawing.Point(146, 388);
            this.txtstock.Name = "txtstock";
            this.txtstock.Size = new System.Drawing.Size(121, 20);
            this.txtstock.TabIndex = 106;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 395);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 108;
            this.label4.Text = "Stock Minimo";
            // 
            // txtnombre
            // 
            this.txtnombre.Location = new System.Drawing.Point(146, 308);
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.Size = new System.Drawing.Size(121, 20);
            this.txtnombre.TabIndex = 104;
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 45);
            this.label2.TabIndex = 107;
            this.label2.Text = "Maestro de Insumos";
            // 
            // txtunidad
            // 
            this.txtunidad.Location = new System.Drawing.Point(146, 418);
            this.txtunidad.Name = "txtunidad";
            this.txtunidad.Size = new System.Drawing.Size(121, 20);
            this.txtunidad.TabIndex = 123;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 425);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 124;
            this.label1.Text = "U. medida";
            // 
            // FormABMinsumos_750VR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtunidad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbnTodos);
            this.Controls.Add(this.rbnActivos);
            this.Controls.Add(this.txtdesc);
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtcant);
            this.Controls.Add(this.txtstock);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtnombre);
            this.Controls.Add(this.label2);
            this.Name = "FormABMinsumos_750VR";
            this.Text = "FormABMinsumos_750VR";
            this.Load += new System.EventHandler(this.FormABMinsumos_750VR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbnTodos;
        private System.Windows.Forms.RadioButton rbnActivos;
        private System.Windows.Forms.TextBox txtdesc;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtcant;
        private System.Windows.Forms.TextBox txtstock;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtunidad;
        private System.Windows.Forms.Label label1;
    }
}