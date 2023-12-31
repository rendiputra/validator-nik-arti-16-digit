﻿using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace validator_nik_arti_16_digit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NikController : ControllerBase
    {
        [HttpGet]
        public IActionResult Nik(long nik)
        {
            string gender;

            if (nik.ToString().Length != 16)
            {
                return BadRequest(new { ErrorMessage = "Jumlah digit NIK tidak valid (NIK harus berisi angka sebanyak 16 digit)" });
            }

            // retrieve  digit nik
            //nik = 3216124310942756;
            long digit2 = (long)(nik / Math.Pow(10, (int)Math.Floor(Math.Log10(nik)) - 1));
            long digit4 = (long)(nik / Math.Pow(10, (int)Math.Floor(Math.Log10(nik)) - 3));
            long digit6 = (long)(nik / Math.Pow(10, (int)Math.Floor(Math.Log10(nik)) - 5));
            string stringDigit2 = Convert.ToString(digit2);
            string stringDigit4 = Convert.ToString(digit4);
            string stringDigit6 = Convert.ToString(digit6);

            // parse json
            JsonNode document       = JsonNode.Parse(json: DataDaerah.jsonDaerah)!;
            JsonNode root           = document.Root;
            JsonNode provinsiNode   = root["Provinsi"];
            string infoProvinsi     = (string)provinsiNode[stringDigit2];
            JsonNode kabKotNode     = root["KabKot"];
            string infoKabKot       = (string)kabKotNode[stringDigit4];
            JsonNode kecamatanNode  = root["Kecamatan"];
            string infoKecamatan    = (string)kecamatanNode[stringDigit6];

            // calculate birthday
            int tgl     = (int)((nik / 100000000) % 100);
            int bulan   = (int)((nik / 1000000) % 100);
            int tahun   = (int)((nik / 10000) % 100);

            // gender
            if (tgl > 40)
            {
                gender = "Perempuan";
                tgl -= 40;
            }
            else
            {
                gender = "Laki-laki";
            }

            //return Ok(dataDaerah?.KabKot);
            return Ok(new
            {
                provinsi = $"{infoProvinsi}",
                kota = $"{infoKabKot}",
                kecamatan = $"{infoKecamatan}",
                gender = gender,
                tanngal = tgl,
                bulan = bulan,
                tahun = tahun
            });
        }
    }
}
