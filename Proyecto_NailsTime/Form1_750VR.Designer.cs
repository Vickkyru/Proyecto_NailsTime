namespace Proyecto_NailsTime
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pestañaAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionPerfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaMaestros = new System.Windows.Forms.ToolStripMenuItem();
            this.ABMclientes = new System.Windows.Forms.ToolStripMenuItem();
            this.ABMservicios = new System.Windows.Forms.ToolStripMenuItem();
            this.ABMhorarios = new System.Windows.Forms.ToolStripMenuItem();
            this.ABMinsumos = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.inicioSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarClave = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarIdioma = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaReserva = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarReserva = new System.Windows.Forms.ToolStripMenuItem();
            this.actualizarAgenda = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaInsumos = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaReportes = new System.Windows.Forms.ToolStripMenuItem();
            this.Facturas = new System.Windows.Forms.ToolStripMenuItem();
            this.pestañaAyuda = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bitacoraEvento = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.RosyBrown;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Right;
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pestañaAdmin,
            this.pestañaMaestros,
            this.pestañaUsuarios,
            this.pestañaReserva,
            this.pestañaInsumos,
            this.pestañaReportes,
            this.pestañaAyuda});
            this.menuStrip1.Location = new System.Drawing.Point(769, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(130, 489);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // pestañaAdmin
            // 
            this.pestañaAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionUsuarios,
            this.gestionPerfiles,
            this.bitacoraEvento});
            this.pestañaAdmin.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaAdmin.Name = "pestañaAdmin";
            this.pestañaAdmin.Size = new System.Drawing.Size(123, 25);
            this.pestañaAdmin.Text = "Administrador";
            this.pestañaAdmin.Click += new System.EventHandler(this.administradorToolStripMenuItem_Click);
            // 
            // gestionUsuarios
            // 
            this.gestionUsuarios.Name = "gestionUsuarios";
            this.gestionUsuarios.Size = new System.Drawing.Size(223, 26);
            this.gestionUsuarios.Text = "Gestion de usuarios";
            this.gestionUsuarios.Click += new System.EventHandler(this.usuariosToolStripMenuItem_Click);
            // 
            // gestionPerfiles
            // 
            this.gestionPerfiles.Name = "gestionPerfiles";
            this.gestionPerfiles.Size = new System.Drawing.Size(223, 26);
            this.gestionPerfiles.Text = "gestion de perfiles";
            this.gestionPerfiles.Click += new System.EventHandler(this.gestionDePerfilesToolStripMenuItem_Click);
            // 
            // pestañaMaestros
            // 
            this.pestañaMaestros.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ABMclientes,
            this.ABMservicios,
            this.ABMhorarios,
            this.ABMinsumos});
            this.pestañaMaestros.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaMaestros.Name = "pestañaMaestros";
            this.pestañaMaestros.Size = new System.Drawing.Size(123, 25);
            this.pestañaMaestros.Text = "Maestros";
            // 
            // ABMclientes
            // 
            this.ABMclientes.Name = "ABMclientes";
            this.ABMclientes.Size = new System.Drawing.Size(146, 26);
            this.ABMclientes.Text = "Clientes";
            this.ABMclientes.Click += new System.EventHandler(this.clientesToolStripMenuItem_Click);
            // 
            // ABMservicios
            // 
            this.ABMservicios.Name = "ABMservicios";
            this.ABMservicios.Size = new System.Drawing.Size(146, 26);
            this.ABMservicios.Text = "Servicios";
            this.ABMservicios.Click += new System.EventHandler(this.serviciosToolStripMenuItem_Click);
            // 
            // ABMhorarios
            // 
            this.ABMhorarios.Name = "ABMhorarios";
            this.ABMhorarios.Size = new System.Drawing.Size(146, 26);
            this.ABMhorarios.Text = "Horarios";
            this.ABMhorarios.Click += new System.EventHandler(this.personalToolStripMenuItem_Click);
            // 
            // ABMinsumos
            // 
            this.ABMinsumos.Name = "ABMinsumos";
            this.ABMinsumos.Size = new System.Drawing.Size(146, 26);
            this.ABMinsumos.Text = "Insumos";
            this.ABMinsumos.Click += new System.EventHandler(this.insumosToolStripMenuItem1_Click);
            // 
            // pestañaUsuarios
            // 
            this.pestañaUsuarios.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inicioSesion,
            this.cambiarClave,
            this.cerrarSesion,
            this.cambiarIdioma});
            this.pestañaUsuarios.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaUsuarios.Name = "pestañaUsuarios";
            this.pestañaUsuarios.Size = new System.Drawing.Size(123, 25);
            this.pestañaUsuarios.Text = "Usuario";
            // 
            // inicioSesion
            // 
            this.inicioSesion.Name = "inicioSesion";
            this.inicioSesion.Size = new System.Drawing.Size(195, 26);
            this.inicioSesion.Text = "Inicio Sesion";
            this.inicioSesion.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // cambiarClave
            // 
            this.cambiarClave.Name = "cambiarClave";
            this.cambiarClave.Size = new System.Drawing.Size(195, 26);
            this.cambiarClave.Text = "Cambiar clave";
            this.cambiarClave.Click += new System.EventHandler(this.cambiarClaveToolStripMenuItem_Click);
            // 
            // cerrarSesion
            // 
            this.cerrarSesion.Name = "cerrarSesion";
            this.cerrarSesion.Size = new System.Drawing.Size(195, 26);
            this.cerrarSesion.Text = "Cerrar Sesion";
            this.cerrarSesion.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // cambiarIdioma
            // 
            this.cambiarIdioma.Name = "cambiarIdioma";
            this.cambiarIdioma.Size = new System.Drawing.Size(195, 26);
            this.cambiarIdioma.Text = "Cambiar Idioma";
            this.cambiarIdioma.Click += new System.EventHandler(this.cambiarIdiomaToolStripMenuItem_Click);
            // 
            // pestañaReserva
            // 
            this.pestañaReserva.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registrarReserva,
            this.actualizarAgenda});
            this.pestañaReserva.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaReserva.Name = "pestañaReserva";
            this.pestañaReserva.Size = new System.Drawing.Size(123, 25);
            this.pestañaReserva.Text = "Reserva";
            // 
            // registrarReserva
            // 
            this.registrarReserva.Name = "registrarReserva";
            this.registrarReserva.Size = new System.Drawing.Size(212, 26);
            this.registrarReserva.Text = "Registrar Reserva";
            this.registrarReserva.Click += new System.EventHandler(this.verTurnosDisponiblesToolStripMenuItem_Click);
            // 
            // actualizarAgenda
            // 
            this.actualizarAgenda.Name = "actualizarAgenda";
            this.actualizarAgenda.Size = new System.Drawing.Size(212, 26);
            this.actualizarAgenda.Text = "Actualizar Agenda";
            this.actualizarAgenda.Click += new System.EventHandler(this.verTurnosReservadosToolStripMenuItem_Click);
            // 
            // pestañaInsumos
            // 
            this.pestañaInsumos.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaInsumos.Name = "pestañaInsumos";
            this.pestañaInsumos.Size = new System.Drawing.Size(123, 25);
            this.pestañaInsumos.Text = "Insumos";
            // 
            // pestañaReportes
            // 
            this.pestañaReportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Facturas});
            this.pestañaReportes.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaReportes.Name = "pestañaReportes";
            this.pestañaReportes.Size = new System.Drawing.Size(123, 25);
            this.pestañaReportes.Text = "Reportes";
            // 
            // Facturas
            // 
            this.Facturas.Name = "Facturas";
            this.Facturas.Size = new System.Drawing.Size(139, 26);
            this.Facturas.Text = "Facturas";
            this.Facturas.Click += new System.EventHandler(this.facturasToolStripMenuItem_Click);
            // 
            // pestañaAyuda
            // 
            this.pestañaAyuda.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pestañaAyuda.Name = "pestañaAyuda";
            this.pestañaAyuda.Size = new System.Drawing.Size(123, 25);
            this.pestañaAyuda.Text = "Ayuda";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Proyecto_NailsTime.Properties.Resources.Captura_de_pantalla_2025_04_21_230616;
            this.pictureBox1.Location = new System.Drawing.Point(769, 404);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 85);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 456);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bienvenido: ";
            // 
            // bitacoraEvento
            // 
            this.bitacoraEvento.Name = "bitacoraEvento";
            this.bitacoraEvento.Size = new System.Drawing.Size(223, 26);
            this.bitacoraEvento.Text = "Bitacora";
            this.bitacoraEvento.Click += new System.EventHandler(this.bitacoraToolStripMenuItem_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(899, 489);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pestañaAdmin;
        private System.Windows.Forms.ToolStripMenuItem gestionUsuarios;
        private System.Windows.Forms.ToolStripMenuItem pestañaMaestros;
        private System.Windows.Forms.ToolStripMenuItem pestañaUsuarios;
        private System.Windows.Forms.ToolStripMenuItem inicioSesion;
        private System.Windows.Forms.ToolStripMenuItem cambiarClave;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem pestañaReserva;
        private System.Windows.Forms.ToolStripMenuItem pestañaInsumos;
        private System.Windows.Forms.ToolStripMenuItem pestañaReportes;
        private System.Windows.Forms.ToolStripMenuItem pestañaAyuda;
        private System.Windows.Forms.ToolStripMenuItem registrarReserva;
        private System.Windows.Forms.ToolStripMenuItem actualizarAgenda;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem ABMclientes;
        private System.Windows.Forms.ToolStripMenuItem ABMservicios;
        private System.Windows.Forms.ToolStripMenuItem ABMhorarios;
        private System.Windows.Forms.ToolStripMenuItem cambiarIdioma;
        private System.Windows.Forms.ToolStripMenuItem gestionPerfiles;
        private System.Windows.Forms.ToolStripMenuItem ABMinsumos;
        private System.Windows.Forms.ToolStripMenuItem Facturas;
        private System.Windows.Forms.ToolStripMenuItem bitacoraEvento;
    }
}

