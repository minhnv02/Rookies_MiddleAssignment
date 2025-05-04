namespace MidAssignment.Domain.Constants
{
    public static class EmailConstants
    {
        public static string BodyActivationEmail(string email) =>
    $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Account Activation</title>
    <style>
        /* Reset styles */
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        /* Container styles */
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        /* Heading styles */
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        /* Paragraph styles */
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        /* Button styles */
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        /* Footer styles */
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Activate Your Account</h1>
        <p>Hello,</p>
        <p>Welcome to Library. Thank you for using our services.</p>
        <p>To experience the service, please activate your account by clicking the button below:</p>
        <p><a href=""http://localhost:5257/api/auths/active-account/{email}"" class=""btn"">Activate Account</a></p>
        <p>If you have any questions or need assistance, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyResetPasswordEmail(string email) =>
            @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Password Reset</title>
    <style>
        /* Reset styles */
        body, html {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }
        /* Container styles */
        .container {
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }
        /* Heading styles */
        h1 {
            font-size: 24px;
            text-align: center;
            color: #333;
        }
        /* Paragraph styles */
        p {
            margin-bottom: 20px;
            color: #666;
        }
        /* Button styles */
        .btn {
            display: inline-block;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }
        /* Footer styles */
        .footer {
            margin-top: 20px;
            text-align: center;
            color: #999;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>We received a request to reset your password. If you did not make this request, you can ignore this email.</p>
        <p>To reset your password, please click the button below:</p>
        <p><a href=""http://localhost:5016/Home/Resetpassword?userId=2}"" class=""btn"">Reset Password</a></p>
        <p>If you have any questions or need assistance, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyBorrowingApprovedEmail(long requestId) =>
    $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Borrowing Approved</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #28a745;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>Your borrowing request with ID {requestId} has been approved!</p>
        <p>Thank you for using our services. If you have any questions, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyBorrowingRejectedEmail(long requestId) =>
    $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Borrowing Rejected</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #dc3545;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>We regret to inform you that your borrowing request with ID {requestId} has been rejected.</p>
        <p>If you have any questions or need assistance, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyExtendBorrowingEmail(long borrowingId, long bookId) =>
    $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Extend Borrowing Request</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #007bff;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>A request has been made to extend the borrowing period for the book with ID {bookId}. Please review the request using the links below:</p>
        <p><a href=""http://localhost:5016/Home/ApproveExtension?borrowingId={borrowingId}&bookId={bookId}"" class=""btn"">Approve Extension</a></p>
        <p><a href=""http://localhost:5016/Home/RejectExtension?borrowingId={borrowingId}&bookId={bookId}"" class=""btn"">Reject Extension</a></p>
        <p>If you have any questions or need assistance, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyApproveExtensionEmail(long borrowingId, long bookId) =>
    $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Extension Approved</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #28a745;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>Your request to extend the borrowing period for the book with ID {bookId} has been approved!</p>
        <p>Borrowing ID: {borrowingId}</p>
        <p>Thank you for using our services. If you have any questions, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public static string BodyRejectExtensionEmail(long borrowingId, long bookId) =>
            $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Extension Rejected</title>
    <style>
        body, html {{
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f9f9f9;
        }}
        h1 {{
            font-size: 24px;
            text-align: center;
            color: #333;
        }}
        p {{
            margin-bottom: 20px;
            color: #666;
        }}
        .btn {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #dc3545;
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            color: #999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p>Hello,</p>
        <p>We regret to inform you that your request to extend the borrowing period for the book with ID {bookId} has been rejected.</p>
        <p>Borrowing ID: {borrowingId}</p>
        <p>If you have any questions or need assistance, please contact our support team.</p>
        <p>Thank you,</p>
        <p>The Support Team</p>
        <div class=""footer"">
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>
";

        public const string SUBJECT_RESET_PASSWORD = "Library-Password Reset";
        public const string SUBJECT_ACTIVE_ACCOUNT = "Library-Active Account";
        public const string SUBJECT_BORROWING_APPROVED = "Library-Borrowing Approved";
        public const string SUBJECT_BORROWING_REJECTED = "Library-Borrowing Rejected";
        public const string SUBJECT_EXTEND_BORROWING = "Library-Extend Borrowing";
        public const string SUBJECT_APPROVE_EXTENSION = "Library-Approve Extension";
        public const string SUBJECT_REJECT_EXTENSION = "Library-Reject Extension";
    }
}
