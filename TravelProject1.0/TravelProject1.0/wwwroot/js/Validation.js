const validType = {
    password: /^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$/,
    phone:/^09[0-9]{8}$/,
    email: /^\w+((-\w+)|(.\w+))@[A-Za-z0-9]+((.|-)[A-Za-z0-9]+).[A-Za-z]+$/,
    birthday:/^\d{4}-\d{2}-\d{2}$/,
    name:/^[A-Za-z\s]+$/,
}

function validation(str, type)
{
    if (str == null) return false;
    if (str.trim().length == 0) return false; 
    return type.test(str);
}

