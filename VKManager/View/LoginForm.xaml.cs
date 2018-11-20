using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VKApi;
using VKApi.Help;
using VKApi.Model;
using xNet;

namespace VKManager.View
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private string login;
        private string pass;
        private GroupModelFull groupModel;
        private PhotoModel photoModel;
        private WallModel wallModel;
        private UploadPhotoModel uploadPhotoModel;

        //Пока не пригодилась
        public GroupModelFull GroupModel
        {
            get
            {
                return groupModel;
            }
        }

        static readonly FileInfo file = new FileInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "login.txt"));
        AutorizationModel autorizationModel;
        public LoginForm()
        {
            InitializeComponent();
            this.Loaded += LoginForm_Loaded;
        }

        private void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
            ReadLoginPassDecript();
        }
        /// <summary>
        /// Кодируем Логин и пароль и сохраняем
        /// </summary>
        private void ReadLoginPassDecript()
        {
            if (file.Exists)
            {
                using (StreamReader stream = new StreamReader(file.FullName))
                {
                    try
                    {
                        string[] name = stream.ReadToEnd().Split('\n');
                        txtLogin.Text = LoginProtector.Decript(name[0], "kdfgjlDS9df");
                        txtPassword.Password = LoginProtector.Decript(name[1], "sfajslkfjfo");
                    }
                    catch (Exception ex)
                    {
                        GlobalConfig.logger.Info("Ошибка чтение файла" + ex.StackTrace);
                    }
                }
            }
            else
            {
                file.Create();
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginSuccess(txtLogin.Text, txtPassword.Password))
            {
                WritelnLoginPassEncrypt();
                //TODO: GroupSuccess() Переместить на главную форму
                if (GroupSuccess())
                {
                    MessageBox.Show("Группа успешно получена");
                    MainWindow.groupModel = groupModel;
                }

                if (WallSuccess())
                {
                    MessageBox.Show("Стена успешно получена");
                    MainWindow.wallModel = wallModel;
                }

                if (PhotoSuccess())
                {
                    MessageBox.Show("Адрес сервера загрузки фотографии успешно получен");
                    MainWindow.photoModel = photoModel;

                    if (UploadSuccess())
                    {
                        MessageBox.Show("Фотография успешно загружена");
                        MainWindow.uploadPhotoModel = uploadPhotoModel;
                    }
                }

                //TODO: Сохранение файла по URL
                //Photo.SavePhotoToPath("https://pbs.twimg.com/media/Dry69uVX4AEDAHA.jpg", "test.jpg");

                this.Owner.IsEnabled = true;
                Close();
            }
            else
            {
                GlobalConfig.logger.Info("Неуспешная попытка входа в Аккаунт");
                MessageBox.Show("Неудачная попытка входа");
            }
        }

        private bool UploadSuccess()
        {
            try
            {
                uploadPhotoModel = Photo.UploadPhoto("photo.jpg", photoModel.response.upload_url);
                GlobalConfig.logger.Info(new String('-', 50) + Serialized<UploadPhotoModel>.GetSerializeString(uploadPhotoModel));
                return true;
            }
            #region Отраработка Ошибок
            //TODO: Нехватает МоделиОшибок при другом JSON ответе сервера
            catch (HttpException ex)
            {
                GlobalConfig.logger.Info("Произошла ошибка при работе с HTTP-сервером: {0}", ex.Message);
                switch (ex.Status)
                {
                    case HttpExceptionStatus.Other:
                        GlobalConfig.logger.Info("Неизвестная ошибка");
                        break;

                    case HttpExceptionStatus.ProtocolError:
                        GlobalConfig.logger.Info("Код состояния: {0}", (int)ex.HttpStatusCode);
                        break;

                    case HttpExceptionStatus.ConnectFailure:
                        GlobalConfig.logger.Info("Не удалось соединиться с HTTP-сервером.");
                        break;

                    case HttpExceptionStatus.SendFailure:
                        GlobalConfig.logger.Info("Не удалось отправить запрос HTTP-серверу.");
                        break;

                    case HttpExceptionStatus.ReceiveFailure:
                        GlobalConfig.logger.Info("Не удалось загрузить ответ от HTTP-сервера.");
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.logger.Error($"Непредвиденная ошибка \n {ex.Message} \r\n {ex.StackTrace} \r\n {ex}");
                //MessageBox.Show(ex.Message);
            }
            return false;
            #endregion
        }

        private bool PhotoSuccess()
        {
            try
            {
                photoModel = Photo.GetWallUploadServer(groupModel.response.items.Select( t => t.id).FirstOrDefault(), GlobalConfig.AccessToken);
                GlobalConfig.logger.Info("PhotoSerialize: " + Serialized<PhotoModel>.GetSerializeString(photoModel));
                return true;
            }
            #region Отраработка Ошибок
            //TODO: Нехватает МоделиОшибок при другом JSON ответе сервера
            catch (HttpException ex)
            {
                GlobalConfig.logger.Info("Произошла ошибка при работе с HTTP-сервером: {0}", ex.Message);
                switch (ex.Status)
                {
                    case HttpExceptionStatus.Other:
                        GlobalConfig.logger.Info("Неизвестная ошибка");
                        break;

                    case HttpExceptionStatus.ProtocolError:
                        GlobalConfig.logger.Info("Код состояния: {0}", (int)ex.HttpStatusCode);
                        break;

                    case HttpExceptionStatus.ConnectFailure:
                        GlobalConfig.logger.Info("Не удалось соединиться с HTTP-сервером.");
                        break;

                    case HttpExceptionStatus.SendFailure:
                        GlobalConfig.logger.Info("Не удалось отправить запрос HTTP-серверу.");
                        break;

                    case HttpExceptionStatus.ReceiveFailure:
                        GlobalConfig.logger.Info("Не удалось загрузить ответ от HTTP-сервера.");
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.logger.Error($"Непредвиденная ошибка \n {ex.Message} \r\n {ex.StackTrace} \r\n {ex}");
                //MessageBox.Show(ex.Message);
            }
            return false;
            #endregion
        }

        private bool WallSuccess()
        {
            try
            {
                wallModel = Wall.GetWalls(autorizationModel.user_id, autorizationModel.access_token, true);
                GlobalConfig.AccessToken = autorizationModel.access_token;
                GlobalConfig.logger.Info("WallSerialize: " + Serialized<WallModel>.GetSerializeString(wallModel));
                return true;
            }
            #region Отраработка Ошибок
            //TODO: Нехватает МоделиОшибок при другом JSON ответе сервера
            catch (HttpException ex)
            {
                GlobalConfig.logger.Info("Произошла ошибка при работе с HTTP-сервером: {0}", ex.Message);
                switch (ex.Status)
                {
                    case HttpExceptionStatus.Other:
                        GlobalConfig.logger.Info("Неизвестная ошибка");
                        break;

                    case HttpExceptionStatus.ProtocolError:
                        GlobalConfig.logger.Info("Код состояния: {0}", (int)ex.HttpStatusCode);
                        break;

                    case HttpExceptionStatus.ConnectFailure:
                        GlobalConfig.logger.Info("Не удалось соединиться с HTTP-сервером.");
                        break;

                    case HttpExceptionStatus.SendFailure:
                        GlobalConfig.logger.Info("Не удалось отправить запрос HTTP-серверу.");
                        break;

                    case HttpExceptionStatus.ReceiveFailure:
                        GlobalConfig.logger.Info("Не удалось загрузить ответ от HTTP-сервера.");
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.logger.Error($"Непредвиденная ошибка \n {ex.Message} \r\n {ex.StackTrace} \r\n {ex}");
                //MessageBox.Show(ex.Message);
            }
            return false;
            #endregion
        }
        //TODO: Переместить на главную форму или еще куда
        private bool GroupSuccess()
        {
            try
            {
                groupModel = Group<GroupModelFull>.GetGouprs(autorizationModel.user_id, autorizationModel.access_token, GlobalConfig.logger.Info);
                return true;
            }
            #region Отраработка Ошибок
            //TODO: Нехватает МоделиОшибок при другом JSON ответе сервера
            catch (HttpException ex)
            {
                GlobalConfig.logger.Info("Произошла ошибка при работе с HTTP-сервером: {0}", ex.Message);
                switch (ex.Status)
                {
                    case HttpExceptionStatus.Other:
                        GlobalConfig.logger.Info("Неизвестная ошибка");
                        break;

                    case HttpExceptionStatus.ProtocolError:
                        GlobalConfig.logger.Info("Код состояния: {0}", (int)ex.HttpStatusCode);
                        break;

                    case HttpExceptionStatus.ConnectFailure:
                        GlobalConfig.logger.Info("Не удалось соединиться с HTTP-сервером.");
                        break;

                    case HttpExceptionStatus.SendFailure:
                        GlobalConfig.logger.Info("Не удалось отправить запрос HTTP-серверу.");
                        break;

                    case HttpExceptionStatus.ReceiveFailure:
                        GlobalConfig.logger.Info("Не удалось загрузить ответ от HTTP-сервера.");
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.logger.Error($"Непредвиденная ошибка \n {ex.Message} \r\n {ex.StackTrace} \r\n {ex}");
                //MessageBox.Show(ex.Message);
            }
            return false;
            #endregion
        }

        private bool LoginSuccess(string login, string pass)
        {
            this.login = login;
            this.pass = pass;
            try
            {
                txtLogin.Text = new MailAddress(txtLogin.Text).Address;
                autorizationModel = AutoRegistration.Login(login, pass);
                GlobalConfig.logger.Info($" Login: {login} success autorized. UserID: {autorizationModel.user_id}");
                MessageBox.Show($"userID:{autorizationModel.user_id} \r\n expires:{autorizationModel.expires_in} \r\n token:{autorizationModel.access_token}");
                GlobalConfig.AccessToken = autorizationModel.access_token;
                return true;
            }
            #region Отраработка Ошибок
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception2FAutorization ex)
            {
                GlobalConfig.logger.Info($"На аккаунте установлена двухуровневая система авторизации \n {ex.Message}");
                DoSomeThing();
            }
            catch (ExceptionCapthaAutorization ex)
            {
                GlobalConfig.logger.Info($"Просит ввести капчу \n {ex.Message}");
                DoSomeThing();
            }
            catch (HttpException ex)
            {
                GlobalConfig.logger.Info("Произошла ошибка при работе с HTTP-сервером: {0}", ex.Message);
                switch (ex.Status)
                {
                    case HttpExceptionStatus.Other:
                        GlobalConfig.logger.Info("Неизвестная ошибка");
                        break;

                    case HttpExceptionStatus.ProtocolError:
                        GlobalConfig.logger.Info("Код состояния: {0}", (int)ex.HttpStatusCode);
                        break;

                    case HttpExceptionStatus.ConnectFailure:
                        GlobalConfig.logger.Info("Не удалось соединиться с HTTP-сервером.");
                        break;

                    case HttpExceptionStatus.SendFailure:
                        GlobalConfig.logger.Info("Не удалось отправить запрос HTTP-серверу.");
                        break;

                    case HttpExceptionStatus.ReceiveFailure:
                        GlobalConfig.logger.Info("Не удалось загрузить ответ от HTTP-сервера.");
                        break;
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.logger.Error($"Непредвиденная ошибка \n {ex.Message} \r\n {ex.StackTrace} \r\n {ex}");
                //MessageBox.Show(ex.Message);
            }
            return false;
            #endregion
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.IsEnabled = true;
            Close();
        }

        private void WritelnLoginPassEncrypt()
        {
            using (StreamWriter stream = File.CreateText(file.FullName))
            {
                stream.WriteLine(LoginProtector.Encrypt(login, "kdfgjlDS9df"));
                stream.WriteLine(LoginProtector.Encrypt(pass, "sfajslkfjfo"));
            }
        }
        private void DoSomeThing()
        {
            throw new NotImplementedException();
        }
    }
}
