using System.Text.Json;
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
            string stringDigit2 = Convert.ToString(digit2);
            string stringDigit4 = Convert.ToString(digit4);

            // parse json
            JsonNode document = JsonNode.Parse(json: DataDaerah.jsonDaerah)!;
            JsonNode root = document.Root;
            JsonNode provinsiNode = root["Provinsi"];
            string infoProvinsi = (string)provinsiNode[stringDigit2];
            JsonNode kabKotNode = root["KabKot"];
            string infoKabKot = (string)kabKotNode[stringDigit4];

            //return Ok(dataDaerah?.KabKot);
            return Ok(new
            {
                provinsi = $"{infoProvinsi}",
                kota = $"{infoKabKot}"
            });
        }
    }
}
