const validType = {
    password: /^(?=.*[A-Za-z])(?=.*d)[A-Za-zd]{8,}$/,
    phonenumber: /^09\d{8}$/,
    email: /^[^@\s]+@[^@\s]+$/,
    birthday: /^\d{4}-\d{2}-\d{2}$/,
    address: /^[a-zA-Z0-9\s,]+$/,
    name:/^[A-Za-z\s]+$/,
}

function validation(str, type)
{
    if (str == null) return false;
    if (str.trim().length == 0) return false; 
    return type.test(str);
}

