﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JualBeli_LIB
{
    public class Pegawai : Database
    {
        #region members
        private string kodePegawai;
        private string nama;
        private DateTime lahir;
        private string alamat;
        private int gaji;
        private string username;
        private string password;
        private Jabatan jabatan;
        #endregion

        public Pegawai()
        {
            KodePegawai = "";
            Nama = "";
            Lahir = DateTime.Now;
            Alamat = "";
            Gaji = 0;
            Username = "";
            Password = "";
            Jabatan = new Jabatan();
        }

        public Pegawai(string kode, string nama, DateTime lahir, string alamat, int gaji, string username, string password, Jabatan jabatan)
        {
            KodePegawai = kode;
            Nama = nama;
            Lahir = lahir;
            Alamat = alamat;
            Gaji = gaji;
            Username = username;
            Password = password;
            Jabatan = jabatan;
        }

        #region properties
        public string KodePegawai { get => kodePegawai; set => kodePegawai = value; }
        public string Nama { get => nama; set => nama = value; }
        public DateTime Lahir { get => lahir; set => lahir = value; }
        public string Alamat { get => alamat; set => alamat = value; }
        public int Gaji { get => gaji; set => gaji = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public Jabatan Jabatan { get => jabatan; set => jabatan = value; }
        #endregion

        public override void Insert()
        {
            string sql = "insert into pegawai(Kodepegawai, nama, tgllahir, alamat, gaji, username, password, idjabatan) values ('" +
                 KodePegawai + "','" +
                 Nama.Replace("'", "\\") + "','" +
                 Lahir.ToString("yyyyMMdd") + "','" +
                 Alamat + "','" +
                 Gaji + "','" +
                 Username + "','" +
                 Password + "','" +
                 Jabatan.IdJabatan + "')";

            Koneksi.ExecuteDML(sql);
        }
        public override void Update()
        {
            string sql = "update pegawai set Nama='" + Nama.Replace("'", "\\") + 
                "', tgllahir='" + Lahir + 
                "',alamat='" + Alamat +
                "',gaji='" + Gaji +
                "',username='" + Username +
                "',password='" + Password +
                "',idjabatan='" + Jabatan + 
                "' where KodePegawai='" + KodePegawai + "'";


            Koneksi.ExecuteDML(sql);
        }
        public override string Delete()
        {
            string sql = "DELETE FROM pelanggan WHERE KodePegawai ='" + KodePegawai + "'";

            try
            {
                Koneksi.ExecuteDML(sql);
                return "1";
            }
            catch (Exception error)
            {
                return error.Message + ", sql : " + sql;
            }
        }

        protected override string QueryCommand(string database = "", string criteria = "", string value = "")
        {
            string sql;

            if(criteria == "")
            {
                sql = "SELECT pegawai.kodepegawai, pegawai.Nama, pegawai.TglLahir, pegawai.Alamat, pegawai.Gaji ,pegawai.Username, pegawai.Password, pegawai.IdJabatan, jabatan.Nama as namaJabatan " +
                    "FROM pegawai INNER JOIN jabatan on pegawai.IdJabatan = jabatan.IdJabatan";
            }
            else
            {
                sql = "SELECT pegawai.kodepegawai, pegawai.Nama, pegawai.TglLahir, pegawai.Alamat, pegawai.Gaji ,pegawai.Username, pegawai.Password, pegawai.IdJabatan, jabatan.Nama as namaJabatan " +
                    "FROM pegawai INNER JOIN jabatan on pegawai.IdJabatan = jabatan.IdJabatan " +
                    " where " + criteria + " LIKE '%" + value + "%'";
            }

            return sql;
        }

        public override ArrayList QueryData(string criteria = "", string value = "")
        {
            string sql = QueryCommand(criteria: criteria, value: value);

            MySqlDataReader result = Koneksi.ExecuteQuery(sql);

            ArrayList listItem = new ArrayList();

            while (result.Read() == true)
            {
                Jabatan jabatan = new Jabatan(result.GetValue(7).ToString(), result.GetValue(8).ToString());
                Pegawai pegawai = new Pegawai(
                    result.GetValue(0).ToString(),
                    result.GetValue(1).ToString(),
                    result.GetDateTime(2),
                    result.GetValue(3).ToString(),
                    result.GetInt32(4),
                    result.GetValue(5).ToString(),
                    result.GetValue(6).ToString(),
                    jabatan);
                listItem.Add(pegawai);
            }

            return listItem;
        }

    }
}
