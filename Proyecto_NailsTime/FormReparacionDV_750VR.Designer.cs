namespace Proyecto_NailsTime
{
    partial class FormReparacionDV_750VR
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
            this.label7 = new System.Windows.Forms.Label();
            this.btnrec = new System.Windows.Forms.Button();
            this.btnres = new System.Windows.Forms.Button();
            this.btnsali = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(175, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(295, 26);
            this.label7.TabIndex = 45;
            this.label7.Text = "INCONSISTENCIA PRESENTE EN LA BD";
            // 
            // btnrec
            // 
            this.btnrec.BackColor = System.Drawing.Color.RosyBrown;
            this.btnrec.FlatAppearance.BorderSize = 0;
            this.btnrec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnrec.Location = new System.Drawing.Point(221, 161);
            this.btnrec.Name = "btnrec";
            this.btnrec.Size = new System.Drawing.Size(164, 23);
            this.btnrec.TabIndex = 91;
            this.btnrec.Text = "Recalcular el DV";
            this.btnrec.UseVisualStyleBackColor = false;
            this.btnrec.Click += new System.EventHandler(this.btnrec_Click);
            // 
            // btnres
            // 
            this.btnres.BackColor = System.Drawing.Color.RosyBrown;
            this.btnres.FlatAppearance.BorderSize = 0;
            this.btnres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnres.Location = new System.Drawing.Point(221, 202);
            this.btnres.Name = "btnres";
            this.btnres.Size = new System.Drawing.Size(164, 23);
            this.btnres.TabIndex = 92;
            this.btnres.Text = "Restore DB";
            this.btnres.UseVisualStyleBackColor = false;
            this.btnres.Click += new System.EventHandler(this.btnres_Click);
            // 
            // btnsali
            // 
            this.btnsali.BackColor = System.Drawing.Color.RosyBrown;
            this.btnsali.FlatAppearance.BorderSize = 0;
            this.btnsali.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsali.Location = new System.Drawing.Point(221, 244);
            this.btnsali.Name = "btnsali";
            this.btnsali.Size = new System.Drawing.Size(164, 23);
            this.btnsali.TabIndex = 93;
            this.btnsali.Text = "Salir";
            this.btnsali.UseVisualStyleBackColor = false;
            this.btnsali.Click += new System.EventHandler(this.btnsali_Click);
            // 
            // FormReparacionDV_750VR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnsali);
            this.Controls.Add(this.btnres);
            this.Controls.Add(this.btnrec);
            this.Controls.Add(this.label7);
            this.Name = "FormReparacionDV_750VR";
            this.Text = "FormReparacionDV_750VR";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnrec;
        private System.Windows.Forms.Button btnres;
        private System.Windows.Forms.Button btnsali;
    }
}