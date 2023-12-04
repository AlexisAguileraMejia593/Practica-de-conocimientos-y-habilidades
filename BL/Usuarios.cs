using DL;
using ML;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuarios
    {
        public static ML.Usuarios GetAll()
        {
            ML.Usuarios usuariosobj = new ML.Usuarios();
            usuariosobj.usuarios = new List<ML.Usuarios>();
            try
            {
                using(DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = (from usuar in context.Usuarios
                                 join Rol in context.Rols on usuar.IdRol equals Rol.IdRol
                                 select new
                                 {
                                     IdUsuario = usuar.UsuariosId,
                                     Nombre = usuar.Nombre,
                                     ApellidoPaterno = usuar.ApellidoPaterno,
                                     ApellidoMaterno = usuar.ApellidoMaterno,
                                     Email = usuar.Email,
                                     Contraseña = usuar.Contraseña,
                                     IdRol = Rol.IdRol,
                                     Tipo = Rol.Tipo
                                 });
                    if(query != null )
                    {
                        foreach (var item in query)
                        {
                            ML.Usuarios usuarios = new ML.Usuarios();
                            usuarios.IdUsuarios = item.IdUsuario;
                            usuarios.Nombre = item.Nombre;
                            usuarios.ApellidoPaterno= item.ApellidoPaterno;
                            usuarios.ApellidoMaterno = item.ApellidoMaterno;
                            usuarios.Email = item.Email;
                            usuarios.Contraseña = item.Contraseña;
                            usuarios.Rol = new ML.Rol();
                            usuarios.Rol.IdRol = item.IdRol;
                            usuarios.Rol.Tipo = item.Tipo;
                            usuariosobj.usuarios.Add(usuarios);
                        }
                    }
                    else
                    {

                    }
                }
            } catch (Exception ex)
            {

            }
            return usuariosobj;
        }
        public static ML.Usuarios Add(ML.Usuarios usuarios)
        {
            try
            {
                using(DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    string hashedPassword = EncryptPassword(usuarios.Contraseña);
                    DL.Usuario usuariosEntity = new DL.Usuario();
                    usuariosEntity.Nombre = usuarios.Nombre;
                    usuariosEntity.ApellidoPaterno = usuarios.ApellidoPaterno;
                    usuariosEntity.ApellidoMaterno = usuarios.ApellidoMaterno;
                    usuariosEntity.Email = usuarios.Email;
                    usuariosEntity.Contraseña = hashedPassword;
                    usuariosEntity.IdRol = usuarios.Rol.IdRol;
                    context.Usuarios.Add(usuariosEntity);
                    int rowsAffected = context.SaveChanges();

                    if (rowsAffected > 0)
                    {
                        return usuarios;
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
        public static ML.Usuarios GetById(int IdUsuarios)
        {
            ML.Usuarios result = null;
            try
            {
                using(DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    DL.Usuario usuarioentity = new DL.Usuario();
                    var query = (from usuar in context.Usuarios
                                 join Rol in context.Rols on usuar.IdRol equals Rol.IdRol
                                 where IdUsuarios == usuar.UsuariosId
                                 select new
                                 {
                                     IdUsuario = usuar.UsuariosId,
                                     Nombre = usuar.Nombre,
                                     ApellidoPaterno = usuar.ApellidoPaterno,
                                     ApellidoMaterno = usuar.ApellidoMaterno,
                                     Email = usuar.Email,
                                     Contraseña = usuar.Contraseña,
                                     IdRol = Rol.IdRol,
                                     Tipo = Rol.Tipo,
                                 });
                    if(query != null)
                    {
                        List<ML.Usuarios> list = new List<ML.Usuarios> ();
                        foreach (var registro in query)
                        {
                            ML.Usuarios usuarioslist = new ML.Usuarios();
                            usuarioslist.IdUsuarios = registro.IdUsuario;
                            usuarioslist.Nombre = registro.Nombre;
                            usuarioslist.ApellidoPaterno = registro.ApellidoPaterno;
                            usuarioslist.ApellidoMaterno = registro.ApellidoMaterno;
                            usuarioslist.Email = registro.Email;
                            usuarioslist.Contraseña = registro.Contraseña;
                            //Boxing
                            object boxingusuarios = usuarioslist;
                            list.Add(usuarioslist);
                        }
                        result = list.FirstOrDefault();
                    }
                    else{

                    }
                }
            } catch (Exception ex)
            {

            }
            return result;
        }
        public static ML.Usuarios GetByEmail(string email)
        {
            ML.Usuarios result = null;
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    DL.Usuario usuarioentity = new DL.Usuario();
                    var query = (from usuar in context.Usuarios
                                 join Rol in context.Rols on usuar.IdRol equals Rol.IdRol
                                 where email == usuar.Email
                                 select new
                                 {
                                     IdUsuario = usuar.UsuariosId,
                                     Nombre = usuar.Nombre,
                                     ApellidoPaterno = usuar.ApellidoPaterno,
                                     ApellidoMaterno = usuar.ApellidoMaterno,
                                     Email = usuar.Email,
                                     Contraseña = usuar.Contraseña,
                                     IdRol = Rol.IdRol,
                                     Tipo = Rol.Tipo,
                                 });
                    if (query != null)
                    {
                        List<ML.Usuarios> list = new List<ML.Usuarios>();
                        foreach (var registro in query)
                        {
                            ML.Usuarios usuarioslist = new ML.Usuarios();
                            usuarioslist.IdUsuarios = registro.IdUsuario;
                            usuarioslist.Nombre = registro.Nombre;
                            usuarioslist.ApellidoPaterno = registro.ApellidoPaterno;
                            usuarioslist.ApellidoMaterno = registro.ApellidoMaterno;
                            usuarioslist.Email = registro.Email;
                            usuarioslist.Contraseña = registro.Contraseña;
                            usuarioslist.Rol = new ML.Rol();
                            usuarioslist.Rol.IdRol = registro.IdRol;
                            usuarioslist.Rol.Tipo = registro.Tipo;
                            //Boxing
                            object boxingusuarios = usuarioslist;
                            list.Add(usuarioslist);
                        }
                        result = list.FirstOrDefault();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public static bool Update(ML.Usuarios usuarios)
        {
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = context.Usuarios.SingleOrDefault(m => m.UsuariosId == usuarios.IdUsuarios);
                    if (query != null)
                    {
                        string hashedPassword = EncryptPassword(usuarios.Contraseña);
                        query.Nombre = usuarios.Nombre;
                        query.ApellidoPaterno = usuarios.ApellidoPaterno;
                        query.ApellidoMaterno = usuarios.ApellidoMaterno;
                        query.Email = usuarios.Email;
                        query.Contraseña = hashedPassword;
                        query.IdRol = usuarios.Rol.IdRol;
                        context.SaveChanges();
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
        public static bool Delete(int IdUsuarios)
        {
            try
            {
                using (DL.PracticaDeConocimientosYHabilidadesContext context = new DL.PracticaDeConocimientosYHabilidadesContext())
                {
                    var query = (from m in context.Usuarios
                                 where m.UsuariosId == IdUsuarios
                                 select m).First();
                    context.Usuarios.Remove(query);
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
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
