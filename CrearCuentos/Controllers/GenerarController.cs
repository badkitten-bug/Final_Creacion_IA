using CrearCuentos.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using RestSharp;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CrearCuentos.Controllers
{
    public class GenerarController : Controller
    {
        public static string _EndPoint = "https://api.openai.com/";
        public static string _URI = "v1/chat/completions";

        private readonly string apiKey = "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";

        public IActionResult Cuento()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Cuento(ConsultaOpenAI modelo)
        {
            if (ModelState.IsValid)
            {
                string resultado = "";
                string resultado2 = "";
                string resultado3 = "";

                string prompt1 = $"Desarrolla una sesion de aprendizaje sobre {modelo.Titulo}, genera el titulo del tema {modelo.Titulo}, la competencia {modelo.Competencia}, area del curso {modelo.Area}, tiempo de la sesion {modelo.TiempoSesion}, grado {modelo.NivelGrado}";
                string prompt2 = $"Desarrollar Ficha de aplicacion sobre el {modelo.Titulo} que contenga 'Nombre del nino, edad, actividades durante la sesion, observaciones adicionales, comentarios adicionales,  firma del docente' ";
                string prompt3 = $"Crea {modelo.CantidadPreguntas} preguntas de comprensión de lectura con sus respectivas respuestas (para chicos de {modelo.NivelGrado}) sobre la sesion de aprendizaje: ";


                resultado = Convert.ToString(consultarOpenAI(prompt1));
                resultado2 = Convert.ToString(consultarOpenAI(prompt2 + resultado));
                resultado3 = Convert.ToString(consultarOpenAI(prompt3 + resultado));


                // Convertir las respuestas a UTF-8
                byte[] resultadoBytes = Encoding.Default.GetBytes(resultado);
                byte[] resultado2Bytes = Encoding.Default.GetBytes(resultado2);
                byte[] resultado3Bytes = Encoding.Default.GetBytes(resultado3);

                resultado = Encoding.UTF8.GetString(resultadoBytes);
                resultado2 = Encoding.UTF8.GetString(resultado2Bytes);
                resultado3 = Encoding.UTF8.GetString(resultado3Bytes);


                // Decodificar las entidades HTML en el resultado
                ViewBag.Resultado = HttpUtility.HtmlDecode(resultado);
                ViewBag.Resultado2 = HttpUtility.HtmlDecode(resultado2);
                ViewBag.Resultado3 = HttpUtility.HtmlDecode(resultado3);

                return View(modelo);
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario con errores de validación
            return View(modelo);
        }


        public string consultarOpenAI(string pregunta)
        {
            string resultado = "";
            var strRespuesta = "";

            // Consumir la API
            var oCliente = new RestClient(_EndPoint);
            var oSolicitud = new RestRequest(_URI, Method.Post);
            oSolicitud.AddHeader("Content-Type", "application/json");
            oSolicitud.AddHeader("Authorization", "Bearer " + apiKey);
            CompletionRequest completionRequest = new CompletionRequest();

            // Creamos el cuerpo de la solicitud
            var oCuerpo = new Request()
            {
                model = "gpt-3.5-turbo",
                messages = new List<Message>()
                {
                    new Message() {
                        role="user",
                        content=pregunta,

                    }
                }
            };

            var jsonString = JsonConvert.SerializeObject(oCuerpo);

            oSolicitud.AddJsonBody(jsonString);

            var oRespuesta = oCliente.Post<Response>(oSolicitud);

            strRespuesta = oRespuesta.choices[0].message.content;

            resultado = strRespuesta.ToString();

            return resultado;
        }



        public async Task<string> GenerateAnswer(string pregunta)
        {
            //string apiKey = "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";
            //string apiKey = "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";
            //string apiKey = "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";
            //string apiKey = "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";
            string apiKey = "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";
            string answer = string.Empty;

            var openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = pregunta;
            completion.MaxTokens = 900;
            var result = await openai.Completions.CreateCompletionsAsync(completion);
            if(result != null)
            {
                foreach(var item in result.Completions) {

                    answer = item.Text;
                
                }
            }


            return answer;

        }



    }

}
