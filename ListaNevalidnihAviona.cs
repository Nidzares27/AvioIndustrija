using AvioIndustrija.Models;
using AvioIndustrija.ViewModels;
using System.Data;

namespace AvioIndustrija
{
    public class ListaNevalidnihAviona
    {
        public NevalidniAvionViewModel NevalidniAvion(DataRow dataRow, string greska)
        {
            var model = new NevalidniAvionViewModel();

            model.ImeAviona = dataRow[0].ToString();
            model.Kompanija = dataRow[1].ToString();
            model.GodinaProizvodnje = dataRow[2].ToString();
            model.VisinaM = dataRow[3].ToString();
            model.ŠirinaM = dataRow[4].ToString();
            model.DužinaM = dataRow[5].ToString();
            model.BrojSjedištaBiznisKlase = dataRow[6].ToString();
            model.BrojSjedištaEkonomskeKlase = dataRow[7].ToString();
            model.NosivostKg = dataRow[8].ToString();
            model.KapacitetRezervoaraL = dataRow[9].ToString();
            model.MaksimalniDometKm = dataRow[10].ToString();
            model.ImageUrl = dataRow[11].ToString();
            model.Greska = greska;

            return model;
        }

        public NevalidniAvionViewModel NevalidniAvionConstraints(Avion avion, string greska)
        {
            var model = new NevalidniAvionViewModel();

            model.ImeAviona = avion.ImeAviona.ToString();
            model.Kompanija = avion.Kompanija.ToString();
            model.GodinaProizvodnje = avion.GodinaProizvodnje.ToString();
            model.VisinaM = avion.VisinaM.ToString();
            model.ŠirinaM = avion.ŠirinaM.ToString();
            model.DužinaM = avion.DužinaM.ToString();
            model.BrojSjedištaBiznisKlase = avion.BrojSjedištaBiznisKlase.ToString();
            model.BrojSjedištaEkonomskeKlase = avion.BrojSjedištaEkonomskeKlase.ToString();
            model.NosivostKg = avion.NosivostKg.ToString();
            model.KapacitetRezervoaraL = avion.KapacitetRezervoaraL.ToString();
            model.MaksimalniDometKm = avion.MaksimalniDometKm.ToString();
            model.ImageUrl = avion.ImageUrl ?? null;
            model.Greska = greska;

            return model;
        }
    }
}
