namespace Proyecto_NailsTime
{
    partial class FormRegistroClientes
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
            this.lblmensaje = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btncrear = new System.Windows.Forms.Button();
            this.btnmod = new System.Windows.Forms.Button();
            this.btnact = new System.Windows.Forms.Button();
            this.btncancelar = new System.Windows.Forms.Button();
            this.btnsalir = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtnom = new System.Windows.Forms.TextBox();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.txtape = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblcantuser = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(19, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(762, 150);
            this.dataGridView1.TabIndex = 68;
            // 
            // lblmensaje
            // 
            this.lblmensaje.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblmensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblmensaje.Location = new System.Drawing.Point(32, 228);
            this.lblmensaje.Name = "lblmensaje";
            this.lblmensaje.Size = new System.Drawing.Size(161, 30);
            this.lblmensaje.TabIndex = 66;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btncrear);
            this.panel1.Controls.Add(this.btnmod);
            this.panel1.Controls.Add(this.btnact);
            this.panel1.Controls.Add(this.btncancelar);
            this.panel1.Controls.Add(this.btnsalir);
            this.panel1.Location = new System.Drawing.Point(423, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 175);
            this.panel1.TabIndex = 64;
            // 
            // btncrear
            // 
            this.btncrear.Location = new System.Drawing.Point(18, 17);
            this.btncrear.Name = "btncrear";
            this.btncrear.Size = new System.Drawing.Size(75, 23);
            this.btncrear.TabIndex = 1;
            this.btncrear.Text = "Crear";
            this.btncrear.UseVisualStyleBackColor = true;
            // 
            // btnmod
            // 
            this.btnmod.Location = new System.Drawing.Point(109, 17);
            this.btnmod.Name = "btnmod";
            this.btnmod.Size = new System.Drawing.Size(75, 23);
            this.btnmod.TabIndex = 4;
            this.btnmod.Text = "Modificar";
            this.btnmod.UseVisualStyleBackColor = true;
            // 
            // btnact
            // 
            this.btnact.Location = new System.Drawing.Point(18, 65);
            this.btnact.Name = "btnact";
            this.btnact.Size = new System.Drawing.Size(75, 23);
            this.btnact.TabIndex = 2;
            this.btnact.Text = "Act/Desact";
            this.btnact.UseVisualStyleBackColor = true;
            // 
            // btncancelar
            // 
            this.btncancelar.Location = new System.Drawing.Point(18, 124);
            this.btncancelar.Name = "btncancelar";
            this.btncancelar.Size = new System.Drawing.Size(75, 23);
            this.btncancelar.TabIndex = 52;
            this.btncancelar.Text = "Cancelar";
            this.btncancelar.UseVisualStyleBackColor = true;
            // 
            // btnsalir
            // 
            this.btnsalir.Location = new System.Drawing.Point(109, 125);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(75, 23);
            this.btnsalir.TabIndex = 54;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 329);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 62;
            this.label7.Text = "Email";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 289);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "Nombre";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 329);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Apellido";
            // 
            // txtnom
            // 
            this.txtnom.Location = new System.Drawing.Point(290, 282);
            this.txtnom.Name = "txtnom";
            this.txtnom.Size = new System.Drawing.Size(100, 20);
            this.txtnom.TabIndex = 48;
            // 
            // txtemail
            // 
            this.txtemail.Location = new System.Drawing.Point(290, 322);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(100, 20);
            this.txtemail.TabIndex = 49;
            // 
            // txtape
            // 
            this.txtape.Location = new System.Drawing.Point(68, 322);
            this.txtape.Name = "txtape";
            this.txtape.Size = new System.Drawing.Size(100, 20);
            this.txtape.TabIndex = 47;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 285);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 59;
            this.label4.Text = "DNI";
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(68, 282);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(100, 20);
            this.txtDNI.TabIndex = 46;
            // 
            // lblcantuser
            // 
            this.lblcantuser.AutoSize = true;
            this.lblcantuser.Location = new System.Drawing.Point(660, 12);
            this.lblcantuser.Name = "lblcantuser";
            this.lblcantuser.Size = new System.Drawing.Size(0, 13);
            this.lblcantuser.TabIndex = 58;
            // 
            // label1
            // 
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(478, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 30);
            this.label1.TabIndex = 57;
            this.label1.Text = "Numero de clientes:";
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 45);
            this.label2.TabIndex = 53;
            this.label2.Text = "Registro Clientes";
            // 
            // FormRegistroClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblmensaje);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtnom);
            this.Controls.Add(this.txtemail);
            this.Controls.Add(this.txtape);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblcantuser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "FormRegistroClientes";
            this.Text = "FormRegistroClientes";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblmensaje;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btncrear;
        private System.Windows.Forms.Button btnmod;
        private System.Windows.Forms.Button btnact;
        private System.Windows.Forms.Button btnsalir;
        private System.Windows.Forms.Button btncancelar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtnom;
        private System.Windows.Forms.TextBox txtemail;
        private System.Windows.Forms.TextBox txtape;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label lblcantuser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}