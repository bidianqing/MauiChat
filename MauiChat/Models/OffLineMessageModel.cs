namespace MauiChat.Models
{
    public class OffLineMessageModel
    {
        public long FromUserId { get; set; }

        public long ToUserId { get; set; }

        /// <summary>
        /// 内容 消息内容可以是多种格式，比如图片/文本/语音/地理位置等
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}
