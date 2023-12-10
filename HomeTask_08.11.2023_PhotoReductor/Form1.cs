using System.Windows.Forms;
using WinFormsApp1;

namespace HomeTask_08._11._2023_PhotoReductor
{
    public partial class Form1 : Form
    {
        private Image loadedImage;
        private string captionText = "Your text";
        private Font captionFont = new Font("Arial", 18);
        private Color captionColor = Color.Red;
        private Point captionLocation = new Point(10, 10);
        private bool isMouseDown = false;


        private PictureBox pictureBox;

        public Form1()
        {
            InitializeComponent();

            Button editButton = new Button()
            {
                Text = "Edit Signature",
                Dock = DockStyle.Bottom,
                Height = 30,
            };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            Button loadButton = new Button()
            {
                Text = "Load Image",
                Dock = DockStyle.Bottom,
                Height = 30,
            };
            loadButton.Click += LoadButton_Click;
            this.Controls.Add(loadButton);

            Button saveButton = new Button()
            {
                Text = "Save Image",
                Dock = DockStyle.Bottom,
                Height = 30,
            };
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);

            pictureBox = new PictureBox()
            {
                BackColor = Color.AliceBlue,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            this.Controls.Add(pictureBox);
        }

        private void SavePictureBoxImage()
        {
            // Проверяем, есть ли изображение в PictureBox
            if (pictureBox.Image == null)
            {
                MessageBox.Show("Нет изображения для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // Открываем диалог сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Изображения (*.png)|*.png|Все файлы (*.*)|*.*";
                saveFileDialog.Title = "Сохранить изображение";
                saveFileDialog.DefaultExt = "png";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Получаем путь к выбранному файлу
                    string filePath = saveFileDialog.FileName;

                    // Сохраняем изображение на рабочий стол
                    pictureBox.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                    MessageBox.Show("Изображение успешно сохранено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void SaveButton_Click(object? sender, EventArgs e)
        {
            SavePictureBoxImage();
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            // saveFileDialog.Filter = "Images (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";
            // if(saveFileDialog.ShowDialog() == DialogResult.OK)
            // {
            //     Image imageToSave = pictureBox.Image as Image;

            //     if (imageToSave != null)
            //     {
            //         // Get the file extension from the selected filter
            //         string fileExtension = Path.GetExtension(saveFileDialog.FileName).ToLower();

            //         // Save the image based on the file extension
            //         switch (fileExtension)
            //         {
            //             case ".jpg":
            //                 imageToSave.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                 break;

            //             case ".png":
            //                 imageToSave.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            //                 break;

            //             case ".bmp":
            //                 imageToSave.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
            //                 break;

            //             default:
            //                 MessageBox.Show("Unsupported file format");
            //                 break;
            //         }
            //     }
            //}
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                captionLocation = new Point(e.X, e.Y);
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.X >= captionLocation.X && e.X <= captionLocation.X + captionFont.SizeInPoints * captionText.Length &&
           e.Y >= captionLocation.Y && e.Y <= captionLocation.Y + captionFont.Height)
            {
                isMouseDown = true;
            }
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (loadedImage != null)
            {
                //pictureBox.Image = loadedImage;
                
                e.Graphics.DrawImage(loadedImage, 5, 5);
                e.Graphics.DrawString(captionText, captionFont, new SolidBrush(captionColor), captionLocation);
            }
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            OpenFileDialog openfl = new OpenFileDialog();
            openfl.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (openfl.ShowDialog() == DialogResult.OK)
            {
                //pictureBox.Load(openfl.FileName);
                loadedImage = Image.FromFile(openfl.FileName);
                this.ClientSize = new Size(loadedImage.Width + 50, loadedImage.Height + 50);
                this.Refresh();
            }
        }

        private void EditButton_Click(object? sender, EventArgs e)
        {
            using (EditCaptureForm form = new EditCaptureForm(captionText, captionFont, captionColor))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    captionText = form.CaptionText;
                    captionFont = form.CaptionFont;
                    captionColor = form.CaptionColor;
                    this.Refresh();
                }
            }
        }
    }
}
