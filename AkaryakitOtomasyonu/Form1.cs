using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// OLUŞTURULAN TXT DOSYALARININ, PROGRAMIN BAŞKA BİR BİLGİSAYARA KAYDEDİLMESİ DURUMUNDA KAYBOLMAMASI İÇİN BU TXT DOSYALARINI PROGRAMIN BİN - DEBUG KLASÖRÜNE KAYDETTİK. 

namespace AkaryakitOtomasyonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void DurumYenile()
        {
            PowerStatus Pil = SystemInformation.PowerStatus;

            //Pil durumunu göster
            switch (Pil.PowerLineStatus)
            {
                case PowerLineStatus.Online:
                    CB_SebekeGucu.Checked = true;
                    break;

                case PowerLineStatus.Offline:
                    CB_SebekeGucu.Checked = false;
                    break;

                case PowerLineStatus.Unknown:
                    CB_SebekeGucu.CheckState = CheckState.Indeterminate;
                    break;
            }

            //Pil yüzdesi
            int pillYuzde = (int)(Pil.BatteryLifePercent * 100);
            if (pillYuzde <= 100)

            {
                progressBar6.Value = pillYuzde;
                label36.Text = "%" + pillYuzde.ToString();
            }
            else
            {
                progressBar6.Value = 0;
            }

            // Kalan süre
            int KSure = Pil.BatteryLifeRemaining;
            if (KSure >= 0) 

            {
                KalanPilSuresi.Text = string.Format("{0} Dk.", KSure / 60);
            }
            else
            {
                KalanPilSuresi.Text = string.Empty; 
            }

            // Pil durumu
            KalanPilYuzdesi.Text = Pil.BatteryChargeStatus.ToString();

        }

        // Bu form içinde kullanılacak olan değişkenleri tanımlamamız gerekir. 
        double D_Benzin95 = 0, D_Benzin97 = 0, D_Dizel = 0, D_EuoruDizel = 0, D_Lpg = 0; // Depoda tutacağımız yakıt miktarlarındaki değişkenlerdir.
        double E_Benzin95 = 0, E_Benzin97 = 0, E_Dizel = 0, E_EuoruDizel = 0, E_Lpg = 0; // Eklenecek olan yakıt miktarlarını tutacak olan değişkenlerdir.
        double F_Benzin95 = 0, F_Benzin97 = 0, F_Dizel = 0, F_EuoruDizel = 0, F_Lpg = 0; // Yakıtların fiyatlarını tutacak olan değişkenlerdir.
        double S_Benzin95 = 0, S_Benzin97 = 0, S_Dizel = 0, S_EuoruDizel = 0, S_Lpg = 0; // Satılmış olan yatkıt miktarlarını depodan düşerek değişkenliğini gösterecek ve tutacaktır.

        private void timer1_Tick(object sender, EventArgs e)
        {
            DurumYenile();
        }

        string[] depobilgileri; // dizi tanımladığımız için köşeli parantez kullanıyoruz ve dizinin ismini veriyoruz.
        string[] fiyatbilgileri;

        // metotlar bir işlemi birden fazla aynı yerde kullanmak ve kod kalabalğından kurtulmak için kullanılır. Metot ismini işlemi yapmak istediğimiz yere yazarsak
        // tanımladığımız metot içindeki işlemi gerçekleştirecektir. 

        private void btn_satis_yap_Click(object sender, EventArgs e)
        {
            S_Benzin95   = double.Parse(numericUpDown1.Value.ToString()); // Numericler dacimal özelliktedir. Double değişkeni direkt decimale çevrilmediği için önce stringe çevirip, sonrasında doublea dönüştürerek benzin değişkenine aktarıyoruz. 
            S_Benzin97   = double.Parse(numericUpDown2.Value.ToString());
            S_Dizel      = double.Parse(numericUpDown3.Value.ToString());
            S_EuoruDizel = double.Parse(numericUpDown4.Value.ToString());
            S_Lpg        = double.Parse(numericUpDown5.Value.ToString());

            if (numericUpDown1.Enabled == true) // 1. numeric aktif olduysa,
            {
                D_Benzin95 = D_Benzin95 - S_Benzin95; // satılan benzin miktarını, depodaki benzin miktarından çıkarıyoruz ve güncel halini depodaki benzin değişkenine aktarıyoruz. 
                label29.Text = Convert.ToString(S_Benzin95 * F_Benzin95); // benzin fiyatını satılan benzin miktarıyla çarpıyoruz ve stringe dönüştürerek, ödenecek tutar labelına yazdırıyoruz. 
            }
            else if (numericUpDown2.Enabled == true) // hangi numeric seçiliyse onu bulana dek döngü devam eder ve numericlerden sadece birinin seçilmesi mümkün olduğu için if else yapsını kullanmalıyız. 
            {
                D_Benzin97 = D_Benzin97 - S_Benzin97;
                label29.Text = Convert.ToString(S_Benzin97 * F_Benzin97);
            }
            else if (numericUpDown3.Enabled == true)
            {
                D_Dizel = D_Dizel - S_Benzin95;
                label29.Text = Convert.ToString(S_Dizel * F_Dizel);
            }
            else if (numericUpDown4.Enabled == true)
            {
                D_Dizel = D_EuoruDizel - S_EuoruDizel;
                label29.Text = Convert.ToString(S_EuoruDizel * F_EuoruDizel);
            }
            else if (numericUpDown5.Enabled == true)
            {
                D_Lpg = D_Lpg - S_Lpg;
                label29.Text = Convert.ToString(S_Lpg * F_Lpg);
            }
            // depobilgileri dizisine satışı yapılan yakıt miktarlarını, depodan düşeceği için, ait oldukları eleman satırlarına yeniden aktarmalıyız ve son halini de depo txt dosyasına yazdırmalıyız. 
            depobilgileri[0] = Convert.ToString(D_Benzin95); // dizimiz string türünde olduğu için ve elemanlarımız da double türünde olduğu için, string ifadeye çevirdik.  
            depobilgileri[1] = Convert.ToString(D_Benzin97);
            depobilgileri[2] = Convert.ToString(D_Dizel);
            depobilgileri[3] = Convert.ToString(D_EuoruDizel);
            depobilgileri[4] = Convert.ToString(D_Lpg); // dizimizin elemanlarını güncellemiş olduk ve sırada depo txt dosyasının güncellenmesi var. O yüzden; 

            System.IO.File.WriteAllLines(Application.StartupPath + "\\depo.txt.txt", depobilgileri); // writealllines tüm satırların üzerine yeniden okunup, yazdırılmasını sağlar. 
            txt_depo_oku();
            txt_depo_yaz();

            progressBar_guncelle();
            numericupdown_value(); // depoda kalan miktar kadar numericler de ona göre artış yapabiliyordu, value değeriyle yeniden güncelledik. 

            // satış yapıldıktan sonra numericlerin temizlenmesi için; 
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // comboboxtaki hangi yakıt türü seçildiyse ona ait numericupdownın açılmasını sağlayacağız. 
        {
            if (comboBox1.Text == "Benzin (95)")
            {
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
            }

            else if (comboBox1.Text == "Benzin (97)")
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
            }

            else if (comboBox1.Text == "Dizel")
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
            }
            else if (comboBox1.Text == "Euro Dizel")
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = true;
                numericUpDown5.Enabled = false;
            }
            else if (comboBox1.Text == "LPG")
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = true;
            }
            numericUpDown1.Value = 0; // oynanan numericlerdeki sayı pasif bırakılmadan önce resetlenmesini sağlar. 
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;

            label29.Text = "___________"; // Her değişiklik yapıldığında da güncel fiyat label nesnesini de temizledik. 
        }

        private void btn_fiyat_guncelle_Click(object sender, EventArgs e)
        {
            try // 2 kez taba bastığında yapı kendiliğinden oluşur. 
            {
                F_Benzin95 = F_Benzin95 + (F_Benzin95 * Convert.ToDouble(textBox6.Text) / 100); // Benzin fiyatının tutulduğu değişken = normal benzinin fiyatı + değişkeni double a çevirdik, 6. boxa yazdığımız zam fiyatını 100 ile bölüp, zamı ekledik. 
                fiyatbilgileri[0] = Convert.ToString(F_Benzin95); // eklenen zamı fiyatbilgilerindeki diziye de string ifadeyle yazdırdık. 
            }
            catch (Exception)
            {
                textBox6.Text = "Hata!";

            }
            try // 2 kez taba bastığında yapı kendiliğinden oluşur. 
            {
                F_Benzin97 = F_Benzin97 + (F_Benzin97 * Convert.ToDouble(textBox7.Text) / 100);  
                fiyatbilgileri[1] = Convert.ToString(F_Benzin97); 
            }
            catch (Exception)
            {
                textBox7.Text = "Hata!";

            }
            try 
            {
                F_Dizel = F_Dizel + (F_Dizel * Convert.ToDouble(textBox8.Text) / 100);  
                fiyatbilgileri[2] = Convert.ToString(F_Dizel); 
            }
            catch (Exception)
            {
                textBox8.Text = "Hata!";
            }

            try
            {
                F_EuoruDizel = F_EuoruDizel + (F_EuoruDizel * Convert.ToDouble(textBox9.Text) / 100);
                fiyatbilgileri[3] = Convert.ToString(F_EuoruDizel);
            }
            catch (Exception)
            {
                textBox9.Text = "Hata!";

            }
            try
            {
                F_Lpg = F_EuoruDizel + (F_Lpg * Convert.ToDouble(textBox10.Text) / 100);
                fiyatbilgileri[4] = Convert.ToString(F_Lpg);
            }
            catch (Exception)
            {
                textBox10.Text = "Hata!";

            }
            System.IO.File.WriteAllLines(Application.StartupPath + "\\fiyat.txt.txt", fiyatbilgileri);
            txt_fiyat_oku(); // txt dosyasındaki güncellenen fiyatları boxlara yazdırmak için önce okuduk. 
            txt_fiyat_yaz(); // okunan elemanların yeni fiyatlarını da boxlara sırasıyla yazdırdık. 

        }


        private void btn_depo_guncelle_Click(object sender, EventArgs e) //depo min 0 ve max 1000 olduğundan ona göre değerler vereceğiz ve sayısal karakter dışında veri girişini engelleyeceğiz.
        {

             try // try bloğu başarılı şekilde çalışmazsa catch bloğu çalıştırılacak çünkü bu bir hata yakala yapısıdır. 
             {
                E_Benzin95 = Convert.ToDouble(textBox1.Text); // 1. dizenin convert işlemini yapıp, eklenecek olan yakıt türü miktarını yazdırdık.
                if (1000 < D_Benzin95 + E_Benzin95 || E_Benzin95 <= 0) // Benzin 95 binden byük ve sıfıra eşit ya da 0'dan küçükse,
                    textBox1.Text = "Hata!"; // boxın textine yazdırmayıp, hata mesajı verdik. 
                else 
                    depobilgileri[0] = Convert.ToString(D_Benzin95 + E_Benzin95); // ancak 1000 den küçük ve 0 dan büyükse Depo benzin ve eklenmiş benzinin toplamı güncel olarak boxa string ifade olarak yazdırılacaktır. 
             }
                       
             catch (Exception) // try bloğu karşılık bulmadığında yine catch ile hata yakalanacak ve hata mesajı verilecektir. 
             {
                    textBox1.Text = "Hata!";
              
             }

             try
             {
                E_Benzin97 = Convert.ToDouble(textBox2.Text); 
                if (1000 < D_Benzin97 + E_Benzin97 || E_Benzin97 <= 0)  
                    textBox2.Text = "Hata!";
                else
                    depobilgileri[1] = Convert.ToString(D_Benzin97 + E_Benzin97); 
             }

             catch (Exception) 
             {
                textBox2.Text = "Hata!";
             }

             try
             {
                E_Dizel = Convert.ToDouble(textBox3.Text);
                if (1000 < D_Dizel + E_Dizel || E_Dizel <= 0) 
                    textBox3.Text = "Hata!";
                else
                    depobilgileri[2] = Convert.ToString(D_Dizel + E_Dizel);
             }

             catch (Exception)
             {
                textBox3.Text = "Hata!";

             }

             try
             {
                E_EuoruDizel = Convert.ToDouble(textBox4.Text);
                if (1000 < D_EuoruDizel + E_EuoruDizel || E_EuoruDizel <= 0)
                    textBox4.Text = "Hata!";
                else
                    depobilgileri[3] = Convert.ToString(D_EuoruDizel + E_EuoruDizel);
             }

             catch (Exception)
             {
                textBox4.Text = "Hata!";

             }

             try
             {
                E_Lpg = Convert.ToDouble(textBox5.Text);
                if (1000 < D_Lpg + E_Lpg || E_Lpg <= 0)
                    textBox5.Text = "Hata!";
                else
                    depobilgileri[4] = Convert.ToString(D_Dizel + E_Lpg);
             }

             catch (Exception)
             {
                textBox5.Text = "Hata!";

             }
            // boxlardan güncellenmiş miktar sayılarının txt dosyasında dizelerin üzerine yazdırılması için; 
            System.IO.File.WriteAllLines(Application.StartupPath + "\\depo.txt.txt", depobilgileri); // wrtiealllines ile tüm dizelere sırasıyla güncel miktarları, txt dosyasındaki miktarların üzerine olduğu gibi yazdırmış oluruz. 
            txt_depo_oku(); // labellardaki güncel miktarları okur.
            txt_depo_yaz(); // labellara gincellenmiş bilgileri yazdırır. 
            progressBar_guncelle(); // güncellenen miktara göre barları da güncelledik. 
            numericupdown_value(); // güncellenen miktara göre numericleri de yeni sayıya kadar çektik. 

           // textBox1.Text = "";
           // textBox2.Text = "";
           // textBox3.Text = "";
           // textBox4.Text = "";
           // textBox5.Text = "";
        }


        private void txt_depo_oku() // dışarıdan bilgi almak istemediğimiz için parantez içini boş bıraktık. 
        {
            depobilgileri = System.IO.File.ReadAllLines(Application.StartupPath + "\\depo.txt.txt"); // bindeki debug klasöründeki depo.txt dosyasındaki her satırı oku ve depo bilgileri
                                                                                                     // dizisine aktar. txt kısmına fazladan bir adet txt yazacağız.

            D_Benzin95 = Convert.ToDouble(depobilgileri[0]); // dizimiz string türünde ve txt dosyası verileri double türünde olduğu için double'a convert ediyoruz.
                                                             // 0. dizideki double'ı getirdik.
            D_Benzin97 = Convert.ToDouble(depobilgileri[1]);
            D_Dizel   = Convert.ToDouble(depobilgileri[2]);
            D_EuoruDizel = Convert.ToDouble(depobilgileri[3]);
            D_Lpg = Convert.ToDouble(depobilgileri[4]);
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 

        }

        private void txt_depo_yaz() // txt_depo_oku'da okuttuğumuz dizileri şimdi de label nesnelerine yazdırmamız gerekiyor.
        {
            label6.Text = D_Benzin95.ToString("N"); // double değişkenli verimizi labela yazdırabilmek için yeniden string ifadeye çevirdik
                                                    // ve n karakteriyle de 2 basamaklı sayıdan sonra virgüllü yazdırdık. Ondalık basamak düzenlemesidir. 
            label7.Text = D_Benzin97.ToString("N");
            label8.Text = D_Dizel.ToString("N");
            label9.Text = D_EuoruDizel.ToString("N");
            label10.Text = D_Lpg.ToString("N");
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 
        }

        private void txt_fiyat_oku()
        {
            fiyatbilgileri = System.IO.File.ReadAllLines(Application.StartupPath + "\\fiyat.txt.txt");
            F_Benzin95 = Convert.ToDouble(fiyatbilgileri[0]);
            F_Benzin97 = Convert.ToDouble(fiyatbilgileri[1]);
            F_Dizel = Convert.ToDouble(fiyatbilgileri[2]);
            F_EuoruDizel = Convert.ToDouble(fiyatbilgileri[3]);
            F_Lpg = Convert.ToDouble(fiyatbilgileri[4]);
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 
        }

        private void txt_fiyat_yaz()
        {
            label16.Text = F_Benzin95.ToString("N");
            label17.Text = F_Benzin97.ToString("N");
            label18.Text = F_Dizel.ToString("N");
            label19.Text = F_EuoruDizel.ToString("N");
            label20.Text = F_Lpg.ToString("N");
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 
        }

        private void progressBar_guncelle() // depoda gösterilen yakıtların miktarını çekerek barlara yansıtılmasını sağlar. 
        {
            progressBar1.Value = Convert.ToInt16(D_Benzin95);
            progressBar2.Value = Convert.ToInt16(D_Benzin97);
            progressBar3.Value = Convert.ToInt16(D_Dizel);
            progressBar4.Value = Convert.ToInt16(D_EuoruDizel);
            progressBar5.Value = Convert.ToInt16(D_Lpg);
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 
        }

        private void numericupdown_value() // numericupdown'ın value, min ve max değerleri sadece decimal değişken türünde tutulur. 
                                           // Burada numericlerin, depoda kalan yakıtların max değerlerine kadar arttırılmasını sağlayacağız. 
        {
            numericUpDown1.Maximum = decimal.Parse(D_Benzin95.ToString()); // double türündeki depodaki veriler string olarak çevirdik çinkü double direkt olarak decimale çevrilmez.
            numericUpDown2.Maximum = decimal.Parse(D_Benzin97.ToString()); // artık string olan ifadeyi parse ile decmiale dönüştürebiliriz. 
            numericUpDown3.Maximum = decimal.Parse(D_Dizel.ToString()); // Dizel depoda 50 L kaldıysa numeric değeri de en fazla 50,00 değerine kadar çıkabilecek. 
            numericUpDown4.Maximum = decimal.Parse(D_EuoruDizel.ToString());
            numericUpDown5.Maximum = decimal.Parse(D_Lpg.ToString());
            // çalıştırabilmek için de metodun ismini formun load kısmına yazmayı unutmuyoruz. 
        }


        private void Form1_Load(object sender, EventArgs e) // bu da bir metottur ve (private) yalnızca bu sınıf içerisinde geçerlidir. Sınıf yani formdur.
                                                            // Void ise dışarıya değer göndermemesi demektir.() içinde yazılanlar dışarıdan hangi bilgilerin alınacağının ifadesidir. 
        {
            this.Text = "AKARYAKIT OTOMASYONU";
            progressBar1.Maximum = 1000; // barların kaç eşit parçaya bölündüğünü ifade eder.
            progressBar2.Maximum = 1000;
            progressBar3.Maximum = 1000;
            progressBar4.Maximum = 1000;
            progressBar5.Maximum = 1000;

            txt_depo_oku(); // form açıldığı zaman depo dosyasındaki verilerin okunmasını sonra da labellara yazdırılmasını sağladık. 
            txt_depo_yaz();
            txt_fiyat_oku();
            txt_fiyat_yaz();

            progressBar_guncelle(); // 1000 parçaya bölünen barların depoda kalan yakıt miktarına göre güncelleyip, doldurur. 

            numericupdown_value();

            // satış yap sekmesindeki yakıt türü comboboxının açılır listelerini, elemanlarını yapacağız;
            string[] yakit_türleri = { "Benzin (95)", "Benzin (97)", "Dizel", "Euro Dizel", "LPG" }; // string dizisini yakit türleri olarak tanımladık ve dizede gösterilecek olan elemanların adını yazdık. 
            comboBox1.Items.AddRange(yakit_türleri); // satış yap boxına tanımlanan dizilerin addrange(dizi elemanlarını ekle demektir) ile gösterilmesini sağladık. 
            // Ayrıca boxın özelliklerinden dropdownstyle'ını dropdownlist yaparak bizim eklediğimiz elemanların haricinde dışarıdan eleman eklenmesini önledik. 

            // şimdi de hangi yakıt türü seçiliyse ona ait numericupdownun aktif kılıp, diğerlerini pasif duruma getireceğiz. 
            numericUpDown1.Enabled = false; // form açılır açılmaz bunlar kapalı konumda olacak. 
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown5.Enabled = false;

            // numericupdownların virgülden sonra 2 basamaklı şekilde gözükmesi için; 
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown2.DecimalPlaces = 2;
            numericUpDown3.DecimalPlaces = 2;
            numericUpDown4.DecimalPlaces = 2;
            numericUpDown5.DecimalPlaces = 2;

            // numericupdownların kaçar kaçar azalacağını ve artacağını belirleyeceğiz; 
            numericUpDown1.Increment = 0.1M; // ondalık yani 2 basamaklı gösterimli yaptıysak m karakterini yazmalıyız.
            numericUpDown2.Increment = 0.1M;
            numericUpDown3.Increment = 0.1M;
            numericUpDown4.Increment = 0.1M;
            numericUpDown5.Increment = 0.1M;

            // numericupdownlara dışarıdan veri girişinin yapılmayıp, sadece 0.1 şeklinde arttırılması için; 
            numericUpDown1.ReadOnly = true;
            numericUpDown2.ReadOnly = true;
            numericUpDown3.ReadOnly = true;
            numericUpDown4.ReadOnly = true;
            numericUpDown5.ReadOnly = true;

            DurumYenile();
            timer1.Enabled = true;

        }
    }
}
