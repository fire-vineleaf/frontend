using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace vllib
{
    public class vllib
    {
        private string _host;
        private string _username;
        private string _password;

        public vllib(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;

        }

        #region "players"
        public SimpleJSON.JSONNode getPlayer()
        {
            string uri = "api.php?a=player";
            return get(uri);
        }

        public SimpleJSON.JSONArray getPlayers()
        {
            string uri = "api.php?a=players";
            return (SimpleJSON.JSONArray)get(uri);
        }

        /// <summary>
        /// gets my invitations to other clans
        /// </summary>
        /// <returns></returns>
        public SimpleJSON.JSONArray getInvitations()
        {
            string uri = "api.php?a=invitations";
            return (SimpleJSON.JSONArray)get(uri);
        }
        #endregion


        #region "clan"

        /// <summary>
        /// creates my new clan -> makes me boss
        /// </summary>
        /// <param name="name"></param>
        public void createClan(string name)
        {
            string json = "{\"name\":\"" + name+ "\"}";
            string uri = "api.php?a=clan";
            post(uri, json);
        }

        /// <summary>
        /// disbands my own clan
        /// </summary>
        public void disbandClan()
        {
            string uri = "api.php?a=disband";
            delete(uri, null);
        }

        /// <summary>
        /// makes me leave my clan
        /// </summary>
        public void leaveClan()
        {
            string uri = "api.php?a=leave";
            post(uri, null);
        }

        /// <summary>
        /// invites a player to my clan
        /// </summary>
        /// <param name="id">playerId</param>
        public void invitePlayer(string id)
        {
            string uri = "api.php?a=invite&id=" + id;
            post(uri, null);
        }

        /// <summary>
        /// gets clan members of any clan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SimpleJSON.JSONArray getClanMembers(string id)
        {
            string uri = "api.php?a=members&id="+id;
            return (SimpleJSON.JSONArray)get(uri);
        }

        #endregion


        #region "messages"

        /// <summary>
        /// deletes a message
        /// </summary>
        /// <param name="id"></param>
        public void deleteMessage(string id)
        {
            string uri = "api.php?a=message&id=" + id;
            delete(uri, null);
        }

        public void createMessage(string to, string subject, string content)
        {
            string json = "{\"subject\":\"" + subject + "\",\"content\":\"" + content + "\",\"participants\":[" + to + "]}";
            string uri = "api.php?a=message";
            post(uri, json);
        }

        public void replyToMessage(int messageId, string reply)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"reply\":\"" + reply + "\"}");
            string json = sb.ToString();
            string uri = "api.php?a=reply&id=" + messageId;
            post(uri, json);
        }

        public SimpleJSON.JSONArray getMessages()
        {
            string uri = "api.php?a=messages";
            return (SimpleJSON.JSONArray)get(uri);
        }

        public SimpleJSON.JSONArray getReplies(int id)
        {
            string uri = "api.php?a=replies&id=" + id;
            return (SimpleJSON.JSONArray)get(uri);
        }

        public SimpleJSON.JSONNode getMessage(int id)
        {
            string uri = "api.php?a=message&id=" + id;
            return get(uri);
        }
        #endregion

        #region "private"
        private SimpleJSON.JSONNode get(string uri)
        {
            HttpWebRequest request = getRequest(uri);
            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return SimpleJSON.JSON.Parse(responseString);
        }

        private void post(string uri, string json)
        {
            anyRequest("POST", uri, json);
        }

        private void delete(string uri, string json)
        {
            anyRequest("DELETE", uri, json);
        }

        private void anyRequest(string method, string uri, string json) {
            HttpWebRequest request = getRequest(uri);
            request.Method = method;
            request.ContentType = "text/json";
            if (json != null)
            {
                var data = Encoding.ASCII.GetBytes(json);
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        }

        private HttpWebRequest getRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_host + uri);
            string token = _username + ":" + _password;
            token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
            request.Headers.Add("Authorization", "Basic " + token);
            return request;
        }
        #endregion

    }


}
