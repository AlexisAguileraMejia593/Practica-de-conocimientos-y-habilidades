using Microsoft.Win32;

namespace BL
{
    public class Medicamentos
    {
        public static ML.Medicamentos GetAll()
        {
            ML.Medicamentos medicamentosobj = new ML.Medicamentos();
            medicamentosobj.medicamentos = new List<ML.Medicamentos>();
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = (from medicam in context.Medicamentos
                                 select new
                                 {
                                     IdMedicamentos = medicam.MedicamentosId,
                                     Sku = medicam.Sku,
                                     Nombre = medicam.Nombre,
                                     Precio = medicam.Precio,
                                     Imagen = medicam.Imagen,
                                 });
                    if (query != null)
                    {
                        foreach (var registro in query)
                        {
                            ML.Medicamentos medicamentos = new ML.Medicamentos();
                            medicamentos.IdMedicamentos = registro.IdMedicamentos;
                            medicamentos.SKU = registro.Sku;
                            medicamentos.Nombre = registro.Nombre;
                            medicamentos.Precio = registro.Precio;
                            medicamentos.Imagen = registro.Imagen;
                            medicamentosobj.medicamentos.Add(medicamentos);
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return medicamentosobj;
        }
        public static ML.Medicamentos Add(ML.Medicamentos medicamentos)
        {
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    DL.Medicamento medicamentoEntity = new DL.Medicamento();
                    medicamentoEntity.Sku = medicamentos.SKU;
                    medicamentoEntity.Nombre = medicamentos.Nombre;
                    medicamentoEntity.Precio = medicamentos.Precio;
                    medicamentoEntity.Imagen = medicamentos.Imagen;

                    context.Medicamentos.Add(medicamentoEntity);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        return medicamentos;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static ML.Medicamentos GetById(int IdMedicamento)
        {
            ML.Medicamentos result = null;
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    DL.Medicamento medicamentoEntity = new DL.Medicamento();
                    var query = (from medicam in context.Medicamentos
                                where IdMedicamento == medicam.MedicamentosId
                                select new
                                {
                                    IdMedicamentos = medicam.MedicamentosId,
                                    Sku = medicam.Sku,
                                    Nombre = medicam.Nombre,
                                    Precio = medicam.Precio,
                                    Imagen = medicam.Imagen
                                });
                    if(query != null)
                    {
                        List<ML.Medicamentos> list = new List<ML.Medicamentos>();
                        foreach(var registro in query)
                        {
                            ML.Medicamentos medicamentoslist = new ML.Medicamentos();
                            medicamentoslist.IdMedicamentos = registro.IdMedicamentos;
                            medicamentoslist.SKU = registro.Sku;
                            medicamentoslist.Nombre = registro.Nombre;
                            medicamentoslist.Precio = registro.Precio;
                            medicamentoslist.Imagen = registro.Imagen;
                            //Boxing
                            object boxedmedicamentos = medicamentoslist;
                            list.Add(medicamentoslist);
                        }
                        result = list.FirstOrDefault();
                    }
                }
            } catch (Exception ex)
            {

            }
            return result;
        }
        public static bool Update(ML.Medicamentos medicamentos)
        {
            try
            {
                using(DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = context.Medicamentos.SingleOrDefault(m => m.MedicamentosId == medicamentos.IdMedicamentos);
                    if(query != null)
                    {
                        query.Sku = medicamentos.SKU;
                        query.Nombre = medicamentos.Nombre;
                        query.Precio = medicamentos.Precio;
                        query.Imagen = medicamentos.Imagen;
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            } catch (Exception ex)
            {
                return false;
            }
        }
        public static bool Delete(int IdMedicamento)
        {
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = (from m in context.Medicamentos
                                 where m.MedicamentosId == IdMedicamento
                                 select m).First();
                    context.Medicamentos.Remove(query);
                    int rowAffected = context.SaveChanges();
                    if (rowAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}