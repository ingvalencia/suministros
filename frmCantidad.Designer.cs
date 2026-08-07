namespace Suministro
{
    partial class frmCantidad
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
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.mxt_cant = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(22, 79);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(67, 25);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(130, 79);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(67, 25);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "Cancelar";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // mxt_cant
            // 
            this.mxt_cant.Location = new System.Drawing.Point(59, 26);
            this.mxt_cant.MaxLength = 10;
            this.mxt_cant.Name = "mxt_cant";
            this.mxt_cant.Size = new System.Drawing.Size(100, 20);
            this.mxt_cant.TabIndex = 0;
            this.mxt_cant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mxt_cant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mxt_cant_KeyPress);
            // 
            // frmCantidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(221, 110);
            this.Controls.Add(this.mxt_cant);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCantidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ingrese cantidad";
            this.Load += new System.EventHandler(this.frmCantidad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox mxt_cant;
    }
}