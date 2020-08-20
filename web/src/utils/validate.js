const isFormValid = ({ email, password, captcha }) => {
    const errors = {};
  
    if (email === "") {
      errors.email = "El nombre de usuario es un campo requerido.";
    }
  
    if (password === "") {
      errors.password = "La contraseña es un campo requerido.";
    }
  
    if(captcha===""){
      errors.captcha = "Se requiere marcar el Captcha.";
    }
  
    return errors;
  };
  
  module.exports = { isFormValid };
  