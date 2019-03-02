using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestWebApp.Model;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace TestWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly IVkApi _vkApi;

        private Random _random;

        private readonly List<string> Chastushki = new List<string>
        {
            "БЫЛ ТУТ WAAAAGH ПРОТИВ ЮДИШЕК\nБЫЛ И Я СРЕДИ БРАТИШЕК\nЗАЛУТАТЬ ТИТАН ХОТЕЛИ\nК ГОРКЕ С МОРКОЙ УЛЕТЕЛИ",
            "ОПА, ОПА, ЭЛЬДАРСКАЯ ЗАСАСАДА\nИМ СЕРЖАНТ НАШ ПРИГЛЯНУЛСЯ, ТАК ЕМУ И НАДО",
            "МЫ НА КАДИЮ ПРИБЫЛИ\nНАС ГВАРДОСЫ ТАМ ПОБИЛИ\nИ ТЕПЕРЬ ВО СЛАВУ КХРОНА\nИЩЕМ РУКИ АББАДОНА",
            "ОПА, ОПА, ПРОСРАЛИ ТРИ ОТРЯДА\nКОМИССАР ИЗДОХ ОТ ПЬЯНСТВА, ТАК ЕМУ И НАДО",
            "ВАРП КРИЧИТ, БУРЛИТ ПРОСТРАНСТВО\nВ СТРАХЕ ДАЖЕ КХРОНА ПАСТВА\nВСЕ В ПОРЯДКЕ, ТЫ ПОЙМИ\nУ СЛААНЕШ ЭТИ ДНИ",
            "ОПА, ОПА, СОРОРИТКА РАДА\nЗЛОЙ ТЗИНЧИТ ПОЕЛ У НУРГЛА, ТАК ЕМУ И НАДО",
            "ТЯН К СЕБЕ ВЧЕРА ПОЗВАЛ\nА ГАНДОНЫ Я НЕ БРАЛ\nЯ Ж НЕ ГРЕБАНЫЙ НУРГЛИТ\nИМПЕРАТОР ЗАЩИТИТ!",
            "ОПА, ОПА, МЕХАНИКУМЫ МАРСА\nПУСТЬ ПРОЙДЕТ ЗДЕСЬ ИНКВИЗИТОР\nБУДЕТ ТЕПЛОТРАССА",
            "ХОРУС ТВАРЬ, ЛОРГАР-ПИЗДЕЦ\nТОЛЬКО МАГНУС МОЛОДЕЦ\nТО УСЛЫШАЛ ЛЕМАН РАСС\nНАТЯНУЛ НА ЖОПУ ГЛАЗ",
            "ОПА, ОПА, ЕБУЧАЯ НАСТОЛКА\nТРИ ОБОССАНЫХ КУЛЬТИСТА ОТЫМЕЛИ ВОЛКА",
            "МЫ ЧАСТУШКИ СОЧИНИЛИ\nПРО ВОЙНУ И ПО РАЗДОР\nИХ ПОСЛУШАЛ ИНКВИЗИТОР\nПРИГЛАСИЛ ВСЕХ НА КОСТЕР!",
            "МЕНЯ В ГВАРДИЮ ПРИЗВАЛИ\nИ ЛАЗГАН СО СКЛАДА ДАЛИ\nНИКОГО ИМ НЕ УБИТЬ\nТОЛЬКО КОШЕК ВЕСЕЛИТЬ",
            "ОПА, ОПА, ВЫПАЛИ ШЕСТЕРКИ\nИМПЕРАТОРУ МОЛИТЬСЯ\nРЕЗКО СТАЛИ ОРКИ",
            "КАК-ТО БЕДНОГО НЕКРОНА\nЗАНЕСЛО К НАМ В БИРЮЛЕВО\nСЕМОК НЕТ, НЕТ СИГАРЕТ\nА ЕГО НЕСУТ В ЦВЕТМЕТ",
            "ОПА, ОПА, СОРОРИТОК К БЛАГУ\nТАУ ПРИВЕСТИ ПЫТАЛИСЬ...\nВ ОБЩЕМ МИР ИХ ПРАХУ",
            "ХАОСИТЫ НАС ДАВИЛИ\nВДРУГ САББАТОН НАМ ВКЛЮЧИЛИ\nКАК ОЧНУЛСЯ - ВОТ ПРИКОЛ\nС ТЗИНЧЕМ Я ИГРАЛ В ФУТБОЛ",
            "ОПА, ОПА, ВАРБОС АРМАГЕДОНА\nС НИМ РАБОТАЕТ ТЕПЕРЬ\nПСИХОЛОГ АББАДОНА",
            "СТРАЗАМИ БЛЕСТЯТ ОЧКИ,\nМАКИЯЖ И КАБЛУЧКИ\nВ ОБЩЕМ ПРИБЫЛ В ТРЕТИЙ ВЗВОД\nКОМИСАР-ЭЛЬДАРОВОД...",
            "ОПА, ОПА, ОПЯТЬ ШАЛИТ АЛЬФАРИЙ\nСКОРО КАТАЧАНСКИХ СВИНОК\nОБНАРУЖИТ МАРИЙ",
            "КАК-ТО РАЗ ОДИН МАГОС\nДРЕВНИЙ ФИЛЬМ ВРАГАМ ОТНЕС\nИ ТЕПЕРЬ НЕКРОНЫ ХОРОМ\nИЩУТ ВСЮДУ САРУ КОНОР",
            "ОПА, ОПА, ЗОГОВА ЗАРАЗА!\nДАЖЕ ОРКИ МАТЕРЯТ\nИСЧАДЬЯ АВТОВАЗА",
            "ВОТ ПРОСПЕРО ДОГОРАЕТ,\nПЕРДАКИ ВЗРЫВАЮТСЯ.\nКОГДА МАГНУС ПРИДАВАЕТ\nШУМНО ПОЛУЧАЕТСЯ",
            "ОПА, ОПА, ПРИРОДА КАТАЧАНА\nВОРОБЕЙ СОЖРАЛ ВАРБОССА\nИ ИЗБИЛ ТИТАНА!",
            "МИМО ДОМА АБАДДОНА\nЯ БЕЗ ШУТОК НЕ ХОЖУ\nТО АКВИЛЛУ НАРИСУЮ\nТО ЛИТАНИЮ СКАЖУ",
            "РАЙВЕЛЬ В ОЗЕРЕ КУПАЛАСЬ\nЛИТОНИЮ ТАМ НАШЛА\nЦЕЛЫЙ ДЕНЬ ЕЁ ЧИТАЛА\nДАЖ НА СТРЕЛЬБЫ НЕ ПОШЛА"
        };


        public CallbackController(IConfiguration configuration, IVkApi vkApi)
        {
            _configuration = configuration;
            _vkApi = vkApi;
            _random = new Random();
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Updates updates)
        {
            // Проверяем, что находится в поле "type" 
            switch (updates.Type)
            {
                // Если это уведомление для подтверждения адреса
                case "confirmation":
                    // Отправляем строку для подтверждения 
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    {

                        // Десериализация
                        var msg = Message.FromJson(new VkResponse(updates.Object));

                        if (msg.Text.Contains("частушк"))
                        {
                            _vkApi.Messages.Send(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = msg.PeerId.Value,
                                Message = Chastushki[_random.Next(0, Chastushki.Count-1)]
                            });
                            break;
                        }
                        else
                        {
                            // Отправим в ответ полученный от пользователя текст
                            _vkApi.Messages.Send(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = msg.PeerId.Value,
                                Message = "ЙА РОДИЛСА!!!!"
                            });
                            break;
                        }
                    }
            }
            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }

    }
}
