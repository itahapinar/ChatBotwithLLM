using OllamaSharp;
using System.Text;
using System.Text.Json;
namespace ChatBotwithLLM
{
    public class Mesaj
    {
        public string Gonderen { get; set; } 
        public string Icerik { get; set; }
    }
    public class Sohbet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Baslik { get; set; }
        public List<Mesaj> Mesajlar { get; set; } = new List<Mesaj>();

        
        public override string ToString()
        {
            return Baslik;
        }
    }
    public partial class Form1 : Form

    {
        private OllamaApiClient ollama;
        private List<Sohbet> tumSohbetler = new List<Sohbet>();
        private Sohbet aktifSohbet;
        private string kayitDosyasi = "sohbet_gecmisi.json";
        public Form1()
        {
            InitializeComponent();
            var uri = new Uri("http://localhost:11434");
            ollama = new OllamaApiClient(uri);

            
            ollama.SelectedModel = "llama3";
        }
        
        private void MesajBalonuEkle(string mesaj, bool kullaniciMi)
        {
            Label baloncuk = new Label();
            baloncuk.Text = mesaj;
            baloncuk.AutoSize = true;

            
            baloncuk.MaximumSize = new Size(flowLayoutPanel1.Width - 30, 0);
            baloncuk.Padding = new Padding(10);
            baloncuk.Margin = new Padding(10);
            baloncuk.Font = new Font("Segoe UI", 11);

            
            if (kullaniciMi)
            {
                baloncuk.BackColor = Color.LightSkyBlue; 
                baloncuk.ForeColor = Color.Black;
            }
            else
            {
                baloncuk.BackColor = Color.WhiteSmoke; 
                baloncuk.ForeColor = Color.Black;
            }

            
            flowLayoutPanel1.Controls.Add(baloncuk);

            
            flowLayoutPanel1.ScrollControlIntoView(baloncuk);
        }

        private async void btn_send_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(richTextBox1.Text)) return;

            
            string kullaniciSorusu = richTextBox1.Text;

            
            if (aktifSohbet == null)
            {
                aktifSohbet = new Sohbet();
                
                string kisaMetin = kullaniciSorusu.Length > 20 ? kullaniciSorusu.Substring(0, 20) + "..." : kullaniciSorusu;

                
                kisaMetin = char.ToUpper(kisaMetin[0]) + kisaMetin.Substring(1);

                
                aktifSohbet.Baslik = $"💬 {DateTime.Now.ToString("HH:mm")} | {kisaMetin}";

                tumSohbetler.Add(aktifSohbet);
                listBox1.Items.Add(aktifSohbet); 
            }

            
            aktifSohbet.Mesajlar.Add(new Mesaj { Gonderen = "Sen", Icerik = kullaniciSorusu });
            MesajBalonuEkle("Sen: " + kullaniciSorusu, true);

            
            richTextBox1.Clear();
            btn_send.Enabled = false;

            try
            {
                
                StringBuilder tamCevap = new StringBuilder();

                
                await foreach (var stream in ollama.GenerateAsync(kullaniciSorusu))
                {
                    tamCevap.Append(stream.Response); 
                }

                
                aktifSohbet.Mesajlar.Add(new Mesaj { Gonderen = "Bot", Icerik = tamCevap.ToString() });
                MesajBalonuEkle("Bot: " + tamCevap.ToString(), false);

                
                SohbetleriKaydet();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Yapay zeka ile iletişimde hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                btn_send.Enabled = true;
            }
        }

        private void btn_newChat_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();

            
            aktifSohbet = null;

            
            MesajBalonuEkle("Bot: Merhaba! Size nasıl yardımcı olabilirim?", false);
        }

        private void SohbetleriKaydet()
        {
            
            string jsonVeri = JsonSerializer.Serialize(tumSohbetler);
            File.WriteAllText(kayitDosyasi, jsonVeri);
        }

        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (listBox1.SelectedItem == null) return;

            
            aktifSohbet = (Sohbet)listBox1.SelectedItem;

            
            flowLayoutPanel1.Controls.Clear();

            
            foreach (var mesaj in aktifSohbet.Mesajlar)
            {
                
                bool kullaniciMi = mesaj.Gonderen == "Sen";

                
                MesajBalonuEkle($"{mesaj.Gonderen}: {mesaj.Icerik}", kullaniciMi);
            }
        }
    }
}
