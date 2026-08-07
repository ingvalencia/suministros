namespace Suministro
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
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
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txt_fact3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_fact2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbn_sal = new System.Windows.Forms.RadioButton();
            this.gpb_fact = new System.Windows.Forms.GroupBox();
            this.txt_fechafin = new System.Windows.Forms.TextBox();
            this.txt_fechaini = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_tot = new System.Windows.Forms.TextBox();
            this.txt_prov = new System.Windows.Forms.TextBox();
            this.txt_imp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_sub = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_fact = new System.Windows.Forms.Label();
            this.txt_fact1 = new System.Windows.Forms.TextBox();
            this.btn_busq = new System.Windows.Forms.Button();
            this.txtCodeBar = new System.Windows.Forms.TextBox();
            this.txt_temp = new System.Windows.Forms.TextBox();
            this.btn_imprimir = new System.Windows.Forms.Button();
            this.btn_cancelar = new System.Windows.Forms.Button();
            this.btn_confirmar = new System.Windows.Forms.Button();
            this.btn_pdfDetalle = new System.Windows.Forms.Button();
            this.btn_pdfResumen = new System.Windows.Forms.Button();
            this.dgv_fact = new System.Windows.Forms.DataGridView();
            this.Factura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Documento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoPaq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoBar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodigoArt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Proyecto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnidadMed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsl_estatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tmr_tiempo = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gpb_fact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fact)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact3);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact2);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.gpb_fact);
            this.splitContainer1.Panel1.Controls.Add(this.lbl_fact);
            this.splitContainer1.Panel1.Controls.Add(this.txt_fact1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_busq);
            this.splitContainer1.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel1_MouseMove);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainer1.Panel2.Controls.Add(this.btn_pdfDetalle);
            this.splitContainer1.Panel2.Controls.Add(this.btn_pdfResumen);
            this.splitContainer1.Panel2.Controls.Add(this.txtCodeBar);
            this.splitContainer1.Panel2.Controls.Add(this.txt_temp);
            this.splitContainer1.Panel2.Controls.Add(this.btn_imprimir);
            this.splitContainer1.Panel2.Controls.Add(this.btn_cancelar);
            this.splitContainer1.Panel2.Controls.Add(this.btn_confirmar);
            this.splitContainer1.Panel2.Controls.Add(this.dgv_fact);
            this.splitContainer1.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_Panel2_MouseMove);
            this.splitContainer1.Size = new System.Drawing.Size(1188, 554);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 0;
            // 
            // txt_fact3
            // 
            this.txt_fact3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact3.BackColor = System.Drawing.Color.White;
            this.txt_fact3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact3.Location = new System.Drawing.Point(966, 16);
            this.txt_fact3.MaxLength = 10;
            this.txt_fact3.Name = "txt_fact3";
            this.txt_fact3.Size = new System.Drawing.Size(94, 20);
            this.txt_fact3.TabIndex = 2;
            this.txt_fact3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact3.TextChanged += new System.EventHandler(this.txt_fact3_TextChanged);
            this.txt_fact3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact3_KeyPress);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(950, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "-";
            // 
            // txt_fact2
            // 
            this.txt_fact2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact2.BackColor = System.Drawing.Color.White;
            this.txt_fact2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact2.Location = new System.Drawing.Point(850, 16);
            this.txt_fact2.MaxLength = 10;
            this.txt_fact2.Name = "txt_fact2";
            this.txt_fact2.Size = new System.Drawing.Size(94, 20);
            this.txt_fact2.TabIndex = 1;
            this.txt_fact2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact2.TextChanged += new System.EventHandler(this.txt_fact2_TextChanged);
            this.txt_fact2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact2_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(834, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "-";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.groupBox1.Controls.Add(this.rbn_sal);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 46);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // rbn_sal
            // 
            this.rbn_sal.AutoSize = true;
            this.rbn_sal.Location = new System.Drawing.Point(9, 17);
            this.rbn_sal.Name = "rbn_sal";
            this.rbn_sal.Size = new System.Drawing.Size(142, 17);
            this.rbn_sal.TabIndex = 1;
            this.rbn_sal.TabStop = true;
            this.rbn_sal.Text = "Suministro de mercancía";
            this.rbn_sal.UseVisualStyleBackColor = true;
            this.rbn_sal.CheckedChanged += new System.EventHandler(this.rbn_sal_CheckedChanged);
            // 
            // gpb_fact
            // 
            this.gpb_fact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpb_fact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gpb_fact.Controls.Add(this.txt_fechafin);
            this.gpb_fact.Controls.Add(this.txt_fechaini);
            this.gpb_fact.Controls.Add(this.label1);
            this.gpb_fact.Controls.Add(this.txt_tot);
            this.gpb_fact.Controls.Add(this.txt_prov);
            this.gpb_fact.Controls.Add(this.txt_imp);
            this.gpb_fact.Controls.Add(this.label2);
            this.gpb_fact.Controls.Add(this.txt_sub);
            this.gpb_fact.Controls.Add(this.label3);
            this.gpb_fact.Controls.Add(this.label4);
            this.gpb_fact.Location = new System.Drawing.Point(12, 55);
            this.gpb_fact.Name = "gpb_fact";
            this.gpb_fact.Size = new System.Drawing.Size(1168, 99);
            this.gpb_fact.TabIndex = 15;
            this.gpb_fact.TabStop = false;
            this.gpb_fact.Text = "Datos de factura";
            // 
            // txt_fechafin
            // 
            this.txt_fechafin.Location = new System.Drawing.Point(776, 71);
            this.txt_fechafin.Name = "txt_fechafin";
            this.txt_fechafin.Size = new System.Drawing.Size(94, 20);
            this.txt_fechafin.TabIndex = 20;
            this.txt_fechafin.Visible = false;
            // 
            // txt_fechaini
            // 
            this.txt_fechaini.Location = new System.Drawing.Point(978, 71);
            this.txt_fechaini.Name = "txt_fechaini";
            this.txt_fechaini.Size = new System.Drawing.Size(94, 20);
            this.txt_fechaini.TabIndex = 19;
            this.txt_fechaini.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Socio de negocios:";
            // 
            // txt_tot
            // 
            this.txt_tot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_tot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_tot.Enabled = false;
            this.txt_tot.Location = new System.Drawing.Point(1043, 19);
            this.txt_tot.MaxLength = 50;
            this.txt_tot.Multiline = true;
            this.txt_tot.Name = "txt_tot";
            this.txt_tot.ReadOnly = true;
            this.txt_tot.Size = new System.Drawing.Size(114, 62);
            this.txt_tot.TabIndex = 14;
            this.txt_tot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txt_prov
            // 
            this.txt_prov.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_prov.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_prov.Enabled = false;
            this.txt_prov.Location = new System.Drawing.Point(106, 21);
            this.txt_prov.MaxLength = 100;
            this.txt_prov.Multiline = true;
            this.txt_prov.Name = "txt_prov";
            this.txt_prov.ReadOnly = true;
            this.txt_prov.Size = new System.Drawing.Size(521, 62);
            this.txt_prov.TabIndex = 11;
            // 
            // txt_imp
            // 
            this.txt_imp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_imp.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_imp.Enabled = false;
            this.txt_imp.Location = new System.Drawing.Point(876, 19);
            this.txt_imp.MaxLength = 50;
            this.txt_imp.Multiline = true;
            this.txt_imp.Name = "txt_imp";
            this.txt_imp.ReadOnly = true;
            this.txt_imp.Size = new System.Drawing.Size(114, 62);
            this.txt_imp.TabIndex = 13;
            this.txt_imp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(631, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Subtotal:";
            // 
            // txt_sub
            // 
            this.txt_sub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_sub.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txt_sub.Enabled = false;
            this.txt_sub.Location = new System.Drawing.Point(686, 19);
            this.txt_sub.MaxLength = 50;
            this.txt_sub.Multiline = true;
            this.txt_sub.Name = "txt_sub";
            this.txt_sub.ReadOnly = true;
            this.txt_sub.Size = new System.Drawing.Size(114, 62);
            this.txt_sub.TabIndex = 12;
            this.txt_sub.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(817, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Impuesto:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1003, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Total:";
            // 
            // lbl_fact
            // 
            this.lbl_fact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_fact.AutoSize = true;
            this.lbl_fact.Location = new System.Drawing.Point(648, 19);
            this.lbl_fact.Name = "lbl_fact";
            this.lbl_fact.Size = new System.Drawing.Size(80, 13);
            this.lbl_fact.TabIndex = 4;
            this.lbl_fact.Text = "No. Factura(s): ";
            // 
            // txt_fact1
            // 
            this.txt_fact1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_fact1.BackColor = System.Drawing.Color.White;
            this.txt_fact1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_fact1.Location = new System.Drawing.Point(734, 16);
            this.txt_fact1.MaxLength = 10;
            this.txt_fact1.Name = "txt_fact1";
            this.txt_fact1.Size = new System.Drawing.Size(94, 20);
            this.txt_fact1.TabIndex = 0;
            this.txt_fact1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_fact1.TextChanged += new System.EventHandler(this.txt_fact1_TextChanged);
            this.txt_fact1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_fact1_KeyPress);
            // 
            // btn_busq
            // 
            this.btn_busq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_busq.Image = global::Suministro.Properties.Resources.Busca;
            this.btn_busq.Location = new System.Drawing.Point(1077, 9);
            this.btn_busq.Name = "btn_busq";
            this.btn_busq.Size = new System.Drawing.Size(103, 40);
            this.btn_busq.TabIndex = 3;
            this.btn_busq.Text = "Buscar";
            this.btn_busq.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_busq.UseVisualStyleBackColor = true;
            this.btn_busq.Click += new System.EventHandler(this.btn_busq_Click);
            // 
            // txtCodeBar
            // 
            this.txtCodeBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCodeBar.Location = new System.Drawing.Point(177, 319);
            this.txtCodeBar.Name = "txtCodeBar";
            this.txtCodeBar.Size = new System.Drawing.Size(100, 20);
            this.txtCodeBar.TabIndex = 5;
            this.txtCodeBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodeBar_KeyPress);
            // 
            // txt_temp
            // 
            this.txt_temp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_temp.BackColor = System.Drawing.Color.White;
            this.txt_temp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_temp.Location = new System.Drawing.Point(816, 319);
            this.txt_temp.MaxLength = 35;
            this.txt_temp.Name = "txt_temp";
            this.txt_temp.Size = new System.Drawing.Size(47, 20);
            this.txt_temp.TabIndex = 4;
            this.txt_temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_temp.Visible = false;
            this.txt_temp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_temp_KeyPress);
            this.txt_temp.MouseLeave += new System.EventHandler(this.txt_temp_MouseLeave);
            // 
            // btn_pdfDetalle
            // 
            this.btn_pdfDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_pdfDetalle.BackColor = System.Drawing.Color.White;
            this.btn_pdfDetalle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_pdfDetalle.Enabled = false;
            this.btn_pdfDetalle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btn_pdfDetalle.FlatAppearance.BorderSize = 1;
            this.btn_pdfDetalle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_pdfDetalle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btn_pdfDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pdfDetalle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pdfDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btn_pdfDetalle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_pdfDetalle.Location = new System.Drawing.Point(650, 315);
            this.btn_pdfDetalle.Name = "btn_pdfDetalle";
            this.btn_pdfDetalle.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btn_pdfDetalle.Size = new System.Drawing.Size(190, 48);
            this.btn_pdfDetalle.TabIndex = 6;
            this.btn_pdfDetalle.Text = "Detalle de Embarque";
            this.btn_pdfDetalle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_pdfDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_pdfDetalle.UseVisualStyleBackColor = false;
            this.btn_pdfDetalle.Click += new System.EventHandler(this.btn_pdfDetalle_Click);
            // 
            // btn_pdfResumen
            // 
            this.btn_pdfResumen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_pdfResumen.BackColor = System.Drawing.Color.White;
            this.btn_pdfResumen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_pdfResumen.Enabled = false;
            this.btn_pdfResumen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btn_pdfResumen.FlatAppearance.BorderSize = 1;
            this.btn_pdfResumen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_pdfResumen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btn_pdfResumen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pdfResumen.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pdfResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btn_pdfResumen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_pdfResumen.Location = new System.Drawing.Point(850, 315);
            this.btn_pdfResumen.Name = "btn_pdfResumen";
            this.btn_pdfResumen.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btn_pdfResumen.Size = new System.Drawing.Size(195, 48);
            this.btn_pdfResumen.TabIndex = 7;
            this.btn_pdfResumen.Text = "Resumen de Embarque";
            this.btn_pdfResumen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_pdfResumen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_pdfResumen.UseVisualStyleBackColor = false;
            this.btn_pdfResumen.Click += new System.EventHandler(this.btn_pdfResumen_Click);
            // 
            // btn_imprimir
            // 
            this.btn_imprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_imprimir.Enabled = false;
            this.btn_imprimir.Image = global::Suministro.Properties.Resources.Imprime;
            this.btn_imprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_imprimir.Location = new System.Drawing.Point(536, 319);
            this.btn_imprimir.Name = "btn_imprimir";
            this.btn_imprimir.Size = new System.Drawing.Size(103, 40);
            this.btn_imprimir.TabIndex = 3;
            this.btn_imprimir.Text = "Imprimir";
            this.btn_imprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_imprimir.UseVisualStyleBackColor = true;
            this.btn_imprimir.Click += new System.EventHandler(this.btn_imprimir_Click);
            // 
            // btn_cancelar
            // 
            this.btn_cancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancelar.Image = global::Suministro.Properties.Resources.Cancela;
            this.btn_cancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancelar.Location = new System.Drawing.Point(1085, 319);
            this.btn_cancelar.Name = "btn_cancelar";
            this.btn_cancelar.Size = new System.Drawing.Size(103, 40);
            this.btn_cancelar.TabIndex = 2;
            this.btn_cancelar.Text = "Cancelar";
            this.btn_cancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_cancelar.UseVisualStyleBackColor = true;
            this.btn_cancelar.Click += new System.EventHandler(this.btn_cancelar_Click);
            // 
            // btn_confirmar
            // 
            this.btn_confirmar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_confirmar.Enabled = false;
            this.btn_confirmar.Image = global::Suministro.Properties.Resources.Ok;
            this.btn_confirmar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_confirmar.Location = new System.Drawing.Point(12, 319);
            this.btn_confirmar.Name = "btn_confirmar";
            this.btn_confirmar.Size = new System.Drawing.Size(103, 40);
            this.btn_confirmar.TabIndex = 1;
            this.btn_confirmar.Text = "Confirma";
            this.btn_confirmar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_confirmar.UseVisualStyleBackColor = true;
            this.btn_confirmar.Click += new System.EventHandler(this.btn_confirmar_Click);
            // 
            // dgv_fact
            // 
            this.dgv_fact.AllowUserToAddRows = false;
            this.dgv_fact.AllowUserToDeleteRows = false;
            this.dgv_fact.AllowUserToResizeColumns = false;
            this.dgv_fact.AllowUserToResizeRows = false;
            this.dgv_fact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_fact.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_fact.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_fact.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Factura,
            this.Documento,
            this.CodigoPaq,
            this.CodigoBar,
            this.CodigoArt,
            this.Descripcion,
            this.CantidadF,
            this.CantidadR,
            this.Subtotal,
            this.Cuenta,
            this.Proyecto,
            this.Caja,
            this.Linea,
            this.UnidadMed});
            this.dgv_fact.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgv_fact.Location = new System.Drawing.Point(2, 3);
            this.dgv_fact.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_fact.MultiSelect = false;
            this.dgv_fact.Name = "dgv_fact";
            this.dgv_fact.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_fact.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_fact.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgv_fact.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_fact.Size = new System.Drawing.Size(1188, 311);
            this.dgv_fact.TabIndex = 0;
            this.dgv_fact.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_fact_CellClick);
            this.dgv_fact.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_fact_CellDoubleClick);
            this.dgv_fact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgv_fact_KeyPress);
            // 
            // Factura
            // 
            this.Factura.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Factura.DataPropertyName = "Factura";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Factura.DefaultCellStyle = dataGridViewCellStyle1;
            this.Factura.DividerWidth = 1;
            this.Factura.Frozen = true;
            this.Factura.HeaderText = "Factura";
            this.Factura.Name = "Factura";
            this.Factura.ReadOnly = true;
            this.Factura.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Factura.Width = 50;
            // 
            // Documento
            // 
            this.Documento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Documento.DataPropertyName = "Documento";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Documento.DefaultCellStyle = dataGridViewCellStyle2;
            this.Documento.DividerWidth = 1;
            this.Documento.Frozen = true;
            this.Documento.HeaderText = "Documento";
            this.Documento.Name = "Documento";
            this.Documento.ReadOnly = true;
            this.Documento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Documento.Width = 69;
            // 
            // CodigoPaq
            // 
            this.CodigoPaq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CodigoPaq.DataPropertyName = "CodigoPaq";
            this.CodigoPaq.DividerWidth = 1;
            this.CodigoPaq.Frozen = true;
            this.CodigoPaq.HeaderText = "Código Paq.";
            this.CodigoPaq.Name = "CodigoPaq";
            this.CodigoPaq.ReadOnly = true;
            this.CodigoPaq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoPaq.Visible = false;
            // 
            // CodigoBar
            // 
            this.CodigoBar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CodigoBar.DataPropertyName = "CodigoBar";
            this.CodigoBar.DividerWidth = 1;
            this.CodigoBar.Frozen = true;
            this.CodigoBar.HeaderText = "Código Bar.";
            this.CodigoBar.Name = "CodigoBar";
            this.CodigoBar.ReadOnly = true;
            this.CodigoBar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CodigoBar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoBar.Width = 62;
            // 
            // CodigoArt
            // 
            this.CodigoArt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CodigoArt.DataPropertyName = "CodigoArt";
            this.CodigoArt.DividerWidth = 1;
            this.CodigoArt.Frozen = true;
            this.CodigoArt.HeaderText = "Codigo Art.";
            this.CodigoArt.Name = "CodigoArt";
            this.CodigoArt.ReadOnly = true;
            this.CodigoArt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CodigoArt.Width = 60;
            // 
            // Descripcion
            // 
            this.Descripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Descripcion.DataPropertyName = "Descripcion";
            this.Descripcion.DividerWidth = 1;
            this.Descripcion.Frozen = true;
            this.Descripcion.HeaderText = "Descripción";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            this.Descripcion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Descripcion.Width = 70;
            // 
            // CantidadF
            // 
            this.CantidadF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CantidadF.DataPropertyName = "CantidadF";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            this.CantidadF.DefaultCellStyle = dataGridViewCellStyle3;
            this.CantidadF.DividerWidth = 1;
            this.CantidadF.Frozen = true;
            this.CantidadF.HeaderText = "Cantidad Fact.";
            this.CantidadF.Name = "CantidadF";
            this.CantidadF.ReadOnly = true;
            this.CantidadF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CantidadF.Width = 75;
            // 
            // CantidadR
            // 
            this.CantidadR.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CantidadR.DataPropertyName = "CantidadR";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.Format = "N6";
            dataGridViewCellStyle4.NullValue = "0.000000";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CantidadR.DefaultCellStyle = dataGridViewCellStyle4;
            this.CantidadR.DividerWidth = 1;
            this.CantidadR.Frozen = true;
            this.CantidadR.HeaderText = "Cantidad Rec.";
            this.CantidadR.Name = "CantidadR";
            this.CantidadR.ReadOnly = true;
            this.CantidadR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CantidadR.Width = 74;
            // 
            // Subtotal
            // 
            this.Subtotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Subtotal.DataPropertyName = "Subtotal";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle5.NullValue = null;
            this.Subtotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.Subtotal.DividerWidth = 1;
            this.Subtotal.HeaderText = "Subtotal";
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.ReadOnly = true;
            this.Subtotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Subtotal.Visible = false;
            // 
            // Cuenta
            // 
            this.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Cuenta.DataPropertyName = "Cuenta";
            this.Cuenta.DividerWidth = 1;
            this.Cuenta.HeaderText = "Cuenta";
            this.Cuenta.Name = "Cuenta";
            this.Cuenta.ReadOnly = true;
            this.Cuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cuenta.Visible = false;
            // 
            // Proyecto
            // 
            this.Proyecto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Proyecto.DataPropertyName = "Proyecto";
            this.Proyecto.DividerWidth = 1;
            this.Proyecto.HeaderText = "Proyecto";
            this.Proyecto.Name = "Proyecto";
            this.Proyecto.ReadOnly = true;
            this.Proyecto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Proyecto.Visible = false;
            // 
            // Caja
            // 
            this.Caja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Caja.DataPropertyName = "Caja";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle6.NullValue = null;
            this.Caja.DefaultCellStyle = dataGridViewCellStyle6;
            this.Caja.DividerWidth = 1;
            this.Caja.Frozen = true;
            this.Caja.HeaderText = "No. Caja";
            this.Caja.MaxInputLength = 35;
            this.Caja.Name = "Caja";
            this.Caja.ReadOnly = true;
            this.Caja.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Caja.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Linea
            // 
            this.Linea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Linea.DataPropertyName = "Linea";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Linea.DefaultCellStyle = dataGridViewCellStyle7;
            this.Linea.DividerWidth = 1;
            this.Linea.HeaderText = "Linea";
            this.Linea.MaxInputLength = 5;
            this.Linea.Name = "Linea";
            this.Linea.ReadOnly = true;
            this.Linea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Linea.Visible = false;
            // 
            // UnidadMed
            // 
            this.UnidadMed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.UnidadMed.DataPropertyName = "UnidadMed";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.LightSteelBlue;
            this.UnidadMed.DefaultCellStyle = dataGridViewCellStyle8;
            this.UnidadMed.DividerWidth = 1;
            this.UnidadMed.Frozen = true;
            this.UnidadMed.HeaderText = "Unidad Med./Emp.";
            this.UnidadMed.MaxInputLength = 15;
            this.UnidadMed.Name = "UnidadMed";
            this.UnidadMed.ReadOnly = true;
            this.UnidadMed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.UnidadMed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UnidadMed.Width = 150;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsl_estatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 532);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1188, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsl_estatus
            // 
            this.tsl_estatus.Name = "tsl_estatus";
            this.tsl_estatus.Size = new System.Drawing.Size(0, 17);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // tmr_tiempo
            // 
            this.tmr_tiempo.Interval = 350;
            this.tmr_tiempo.Tick += new System.EventHandler(this.tmr_tiempo_Tick);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1188, 554);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Suministro Mercancías v 2.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpb_fact.ResumeLayout(false);
            this.gpb_fact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fact)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbn_sal;
        private System.Windows.Forms.DataGridView dgv_fact;
        private System.Windows.Forms.TextBox txt_fact1;
        private System.Windows.Forms.Button btn_busq;
        private System.Windows.Forms.Label lbl_fact;
        private System.Windows.Forms.TextBox txt_tot;
        private System.Windows.Forms.TextBox txt_imp;
        private System.Windows.Forms.TextBox txt_sub;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_prov;
        private System.Windows.Forms.GroupBox gpb_fact;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsl_estatus;
        private System.Windows.Forms.Button btn_confirmar;
        private System.Windows.Forms.Button btn_cancelar;
        private System.Windows.Forms.Button btn_imprimir;
        private System.Windows.Forms.Button btn_pdfDetalle;
        private System.Windows.Forms.Button btn_pdfResumen;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TextBox txt_fechafin;
        private System.Windows.Forms.TextBox txt_fechaini;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_temp;
        private System.Windows.Forms.Timer tmr_tiempo;
        private System.Windows.Forms.TextBox txt_fact3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_fact2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCodeBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Factura;
        private System.Windows.Forms.DataGridViewTextBoxColumn Documento;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoPaq;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoArt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadF;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadR;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Proyecto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caja;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidadMed;
    }
}

