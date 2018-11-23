using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Help
{
    public enum GrantType
    {
        password,
        client_credentials
    }

    public enum Attachments
    {
        none,
        photo, // — фотография;
        video, // — видеозапись;
        audio, // — аудиозапись;
        doc, // — документ;
        page, // — wiki-страница;
        note, // — заметка;
        poll, // — опрос;
        album, // — альбом;
        market, // — товар;
        market_album, // — подборка товаров;
        audio_playlist, // — плейлист с аудио.
    }

    public class GetAttachment
    {
        public GetAttachment(int owner_id, int media_id, Attachments attachments)
        {

        }
    }
}
