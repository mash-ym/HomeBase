using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HomeBase
{
    public partial class InitialSetupForm : Form
    {
        private TextBox companyNameTextBox;
        private TextBox companyAddressTextBox;
        private TextBox userEmailTextBox;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private CheckBox complexityCheckBox;
        private CheckBox expirationCheckBox;
        private ComboBox roleComboBox;
        private ListBox securityGroupListBox;
        private TextBox apiKeyTextBox;
        private TextBox authTokenTextBox;
        private TextBox endpointUrlTextBox;

        // 初期設定データの保存や処理を行うオブジェクト
        private InitialSetupData initialSetupData;

        public InitialSetupForm()
        {
            InitializeComponent();
            initialSetupData = new InitialSetupData();
        }

        private void InitializeComponent()
        {
            // 各入力コントロールの作成と配置などの設定を行う

            // companyNameTextBox
            companyNameTextBox = new TextBox();
            // ...

            // companyAddressTextBox
            companyAddressTextBox = new TextBox();
            // ...

            // userEmailTextBox
            userEmailTextBox = new TextBox();
            // ...

            // usernameTextBox
            usernameTextBox = new TextBox();
            // ...

            // passwordTextBox
            passwordTextBox = new TextBox();
            // ...

            // complexityCheckBox
            complexityCheckBox = new CheckBox();
            // ...

            // expirationCheckBox
            expirationCheckBox = new CheckBox();
            // ...

            // roleComboBox
            roleComboBox = new ComboBox();
            // ...

            // securityGroupListBox
            securityGroupListBox = new ListBox();
            // ...

            // apiKeyTextBox
            apiKeyTextBox = new TextBox();
            // ...

            // authTokenTextBox
            authTokenTextBox = new TextBox();
            // ...

            // endpointUrlTextBox
            endpointUrlTextBox = new TextBox();
            // ...

            // 保存ボタン
            Button saveButton = new Button();
            saveButton.Location = new System.Drawing.Point(50, 500);
            saveButton.Text = "保存";
            saveButton.Click += SaveButton_Click;
            this.Controls.Add(saveButton);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                // 入力データを取得
                InitialSetupData data = new InitialSetupData
                {
                    CompanyName = companyNameTextBox.Text,
                    CompanyAddress = companyAddressTextBox.Text,
                    UserEmail = userEmailTextBox.Text,
                    Username = usernameTextBox.Text,
                    Password = passwordTextBox.Text,
                    PasswordComplexity = complexityCheckBox.Checked,
                    PasswordExpiration = expirationCheckBox.Checked,
                    Role = roleComboBox.SelectedItem?.ToString(),
                    SecurityGroups = securityGroupListBox.SelectedItems.Cast<string>().ToList(),
                    ApiKey = apiKeyTextBox.Text,
                    AuthToken = authTokenTextBox.Text,
                    EndpointUrl = endpointUrlTextBox.Text
                };
                DBManager dbManager = new DBManager();
                // データを保存
                DataStorage dataStorage = new DataStorage(dbManager);
                dataStorage.SaveInitialSetupData(data);

                MessageBox.Show("初期設定データが保存されました。");
                Close();
            }
            else
            {
                MessageBox.Show("入力内容に誤りがあります。正しい値を入力してください。");
            }
        }


        private bool ValidateInputs()
        {
            // 各入力コントロールの値のバリデーションを行う
            // 必須項目のチェック、フォーマットの検証などを実装する
            // 不正な値があれば false を返す

            return true;
        }

    }

    public class InitialSetupData
    {
        // 初期設定データのプロパティを定義する
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string UserEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // セキュリティ設定のプロパティを定義する
        public bool PasswordComplexity { get; set; }
        public bool PasswordExpiration { get; set; }
        public string Role { get; set; }
        public List<string> SecurityGroups { get; set; }

        // 外部連携設定のプロパティを定義する
        public string ApiKey { get; set; }
        public string AuthToken { get; set; }
        public string EndpointUrl { get; set; }
    }

}
