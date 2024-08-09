using AvioIndustrija.Controllers;
using AvioIndustrija.Models;
using AvioIndustrija.ViewModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AvioIndustrija.Services
{
    public class ExcelService
    {
        private readonly ListaNevalidnihAviona _listaNevalidnihAviona;
        private readonly HttpClient _httpClient;

        public ExcelService(ListaNevalidnihAviona listaNevalidnihAviona, HttpClient httpClient) //HttpClient httpClient
        {
            this._listaNevalidnihAviona = listaNevalidnihAviona;
            this._httpClient = httpClient;
        }
        public Validni_NevalidniAvioni ImportFromExcelAsync(Stream fileStream) //ImportFromExcel
        {
            var list = new List<Avion>();
            var listaNevalidnihAviona = new List<NevalidniAvionViewModel>();


            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var result = reader.AsDataSet();
                var dataTable = result.Tables[0];

                for (int i = 1; i < dataTable.Rows.Count; i++) // assuming the first row is headers
                {
                    var row = dataTable.Rows[i];
                    var model = new Avion();

                    model.ImeAviona = row[0].ToString();
                    if (model.ImeAviona == "" || String.IsNullOrWhiteSpace(model.ImeAviona))
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "ImeAviona je obavezno"));
                        continue;
                    }
                    model.Kompanija = row[1].ToString();
                    if (model.Kompanija == "" || String.IsNullOrWhiteSpace(model.Kompanija))
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "Kompanija je obavezno"));
                        continue;
                    }

                    if (short.TryParse(row[2].ToString(), out short GodinaProizvodnje))
                    {
                        model.GodinaProizvodnje = GodinaProizvodnje;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "GodinaProizvodnje mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[3].ToString(), out short VisinaM))
                    {
                        model.VisinaM = VisinaM;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "VisinaM mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[4].ToString(), out short ŠirinaM))
                    {
                        model.ŠirinaM = ŠirinaM;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "ŠirinaM mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[5].ToString(), out short DužinaM))
                    {
                        model.DužinaM = DužinaM;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "DužinaM mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[6].ToString(), out short BrojSjedištaBiznisKlase))
                    {
                        model.BrojSjedištaBiznisKlase = BrojSjedištaBiznisKlase;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "BrojSjedištaBiznisKlase mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[7].ToString(), out short BrojSjedištaEkonomskeKlase))
                    {
                        model.BrojSjedištaEkonomskeKlase = BrojSjedištaEkonomskeKlase;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "BrojSjedištaEkonomskeKlase mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[8].ToString(), out short NosivostKg))
                    {
                        model.NosivostKg = NosivostKg;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "NosivostKg mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[9].ToString(), out short KapacitetRezervoaraL))
                    {
                        model.KapacitetRezervoaraL = KapacitetRezervoaraL;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "KapacitetRezervoaraL mora biti broj"));
                        continue;
                    }
                    if (short.TryParse(row[10].ToString(), out short MaksimalniDometKm))
                    {
                        model.MaksimalniDometKm = MaksimalniDometKm;
                    }
                    else
                    {
                        listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "MaksimalniDometKm mora biti broj"));
                        continue;
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////


                    if (String.IsNullOrEmpty(row[11].ToString()))
                    {
                        model.ImageUrl = null;
                    }
                    else
                    {
                        bool isValidImage = IsImageUrlAsync(row[11].ToString()).Result;
                        model.ImageUrl = row[11].ToString();

                        if (!isValidImage)
                        {
                            listaNevalidnihAviona.Add(_listaNevalidnihAviona.NevalidniAvion(row, "Nevalidan ImageURL!"));
                            continue;
                        }
                    }

                    list.Add(model);
                }
            }
            var listaValidnih_Nevalidnih = new Validni_NevalidniAvioni();
            listaValidnih_Nevalidnih.nevalidni = listaNevalidnihAviona;
            listaValidnih_Nevalidnih.validni = list;
            return listaValidnih_Nevalidnih;
        }


        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool HasImageFileExtension(string url)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            string fileExtension = Path.GetExtension(url)?.ToLowerInvariant();
            return validExtensions.Contains(fileExtension);
        }

        private async Task<bool> IsImageUrlAsync(string url)
        {
            if (!IsValidUrl(url) || !HasImageFileExtension(url))
            {
                return false;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string contentType = response.Content.Headers.ContentType.MediaType;
                        return contentType.StartsWith("image/");
                    }
                }
            }
            catch
            {
                // Handle exceptions if needed
            }

            return false;
        }
    }
}
