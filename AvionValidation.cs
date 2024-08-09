using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Services;
using AvioIndustrija.Utils;
using AvioIndustrija.ViewModels;
using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

namespace AvioIndustrija
{
    public class AvionValidation
    {
        private readonly ListaNevalidnihAviona _listaNevalidnihAviona;
        private readonly HttpClient _httpClient;
        private readonly UploadImageFromExcel _uploadImageFromExcel;
        private readonly IPhotoService _photoService;

        public AvionValidation(ListaNevalidnihAviona listaNevalidnihAviona, HttpClient httpClient, UploadImageFromExcel uploadImageFromExcel, IPhotoService photoService)
        {
            this._listaNevalidnihAviona = listaNevalidnihAviona;
            this._httpClient = httpClient;
            this._uploadImageFromExcel = uploadImageFromExcel;
            this._photoService = photoService;
        }
        public async Task<Validni_NevalidniAvioni> ValidateAvions(Validni_NevalidniAvioni avioniZaDodavanje, IEnumerable<Avion> sviAvioni)
        {
            string currentYear = DateTime.Now.Year.ToString();
            int currYear = Int32.Parse(currentYear);

            var azuriranaListaZaDodavanje = new List<Avion>();


            foreach (var item in avioniZaDodavanje.validni)
            {
                bool istoIme = false;
                if (item.GodinaProizvodnje < 1950 || item.GodinaProizvodnje > currYear)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "GodinaProizvodnje mora biti od 1950 do " + currYear));
                    continue;
                }
                if (item.VisinaM < 20 || item.VisinaM > 60)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "VisinaM mora biti od 20 do 60"));
                    continue;
                }
                if (item.ŠirinaM < 20 || item.ŠirinaM > 150)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "ŠirinaM mora biti od 20 do 150"));
                    continue;
                }
                if (item.DužinaM < 20 || item.DužinaM > 150)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "DužinaM mora biti od 20 do 150"));
                    continue;
                }
                if (item.BrojSjedištaBiznisKlase < 0 || item.BrojSjedištaBiznisKlase > 200)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "BrojSjedištaBiznisKlase mora biti od 0 do 200"));
                    continue;
                }
                if (item.BrojSjedištaEkonomskeKlase < 0 || item.BrojSjedištaEkonomskeKlase > 300)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "BrojSjedištaEkonomskeKlase mora biti od 0 do 300"));
                    continue;
                }
                if (item.NosivostKg < 1000 || item.NosivostKg > 10000)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "NosivostKg mora biti od 1000 do 10000"));
                    continue;
                }
                if (item.KapacitetRezervoaraL < 500 || item.KapacitetRezervoaraL > 5000)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "KapacitetRezervoaraL mora biti od 500 do 5000"));
                    continue;
                }
                if (item.MaksimalniDometKm < 1000 || item.MaksimalniDometKm > 5000)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "MaksimalniDometKm mora biti od 1000 do 5000"));
                    continue;
                }

                foreach (var av in sviAvioni)
                {
                    if (istoIme == true) break;
                    if (av.ImeAviona == item.ImeAviona & av.Kompanija == item.Kompanija)
                    {
                        istoIme = true;
                        continue;
                    }
                }

                if (istoIme)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "Kombinacija Imena Aviona i Kompanije vec postoji"));
                    continue;
                }

                foreach (var av in azuriranaListaZaDodavanje)
                {
                    if (istoIme == true) break;
                    if (av.ImeAviona == item.ImeAviona & av.Kompanija == item.Kompanija)
                    {
                        istoIme = true;
                        continue;
                    }
                }

                if (istoIme)
                {
                    avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "Avion sa istim imenom vec postoji u Excel fajlu!"));
                    continue;
                }

                ////////////////////////////////////////////////
                if (!String.IsNullOrEmpty(item.ImageUrl))
                {
                    var imgDownload = await _uploadImageFromExcel.DownloadImage(item.ImageUrl);
                    if (String.IsNullOrEmpty(imgDownload))
                    {
                        avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "Greska prilikom uploada slike!"));
                        continue;
                    }
                    Console.WriteLine("SLIKA USPJESNO UPLOADOVANA!");
                    var cloudinaryFilePath = await _photoService.UploadImage(imgDownload);
                    if (String.IsNullOrEmpty(cloudinaryFilePath))
                    {
                        avioniZaDodavanje.nevalidni.Add(_listaNevalidnihAviona.NevalidniAvionConstraints(item, "Greska prilikom uploada slike u Cloudinary!"));
                        continue;
                    }
                    Console.WriteLine("SLIKA USPJESNO UPLOADOVANA U CLOUDINARY!");
                    item.ImageUrl = cloudinaryFilePath;
                }


                azuriranaListaZaDodavanje.Add(item);
            }

            var listaValidnih_Nevalidnih = new Validni_NevalidniAvioni();
            listaValidnih_Nevalidnih.nevalidni = avioniZaDodavanje.nevalidni;
            listaValidnih_Nevalidnih.validni = azuriranaListaZaDodavanje;

            return listaValidnih_Nevalidnih;
        }
    }
}
