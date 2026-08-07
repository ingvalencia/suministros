namespace Suministro
{
    partial class frmAutenticacion
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
            this.btn_si = new System.Windows.Forms.Button();
            this.btn_no = new System.Windows.Forms.Button();
            this.txt_usr = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_si
            // 
            this.btn_si.Location = new System.Drawing.Point(33, 119);
            this.btn_si.Name = "btn_si";
            this.btn_si.Size = new System.Drawing.Size(75, 23);
            this.btn_si.TabIndex = 2;
            this.btn_si.Text = "Validar";
            this.btn_si.UseVisualStyleBackColor = true;
            this.btn_si.Click += new System.EventHandler(this.btn_si_Click);
            // 
            // btn_no
            // 
            this.btn_no.Location = new System.Drawing.Point(156, 121);
            this.btn_no.Name = "btn_no";
            this.btn_no.Size = new System.Drawing.Size(75, 23);
            this.btn_no.TabIndex = 3;
            this.btn_no.Text = "Cancelar";
            this.btn_no.UseVisualStyleBackColor = true;
            this.btn_no.Click += new System.EventHandler(this.btn_no_Click);
            // 
            // txt_usr
            // 
            this.txt_usr.Location = new System.Drawing.Point(33, 28);
            this.txt_usr.Name = "txt_usr";
            this.txt_usr.PasswordChar = '*';
            this.txt_usr.Size = new System.Drawing.Size(198, 20);
            this.txt_usr.TabIndex = 0;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Location = new System.Drawing.Point(33, 81);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(198, 20);
            this.txt_pwd.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Usuario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(30, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Contraseña";
            // 
            // frmAutenticacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(263, 156);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.txt_usr);
            this.Controls.Add(this.btn_no);
            this.Controls.Add(this.btn_si);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAutenticacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credenciales";
            this.Load += new System.EventHandler(this.frmAutenticacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_si;
        private System.Windows.Forms.Button btn_no;
        private System.Windows.Forms.TextBox txt_usr;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}