import { useState, useEffect } from "react";

const isEmpty = obj => Object.keys(obj).length === 0;

const useForm = (initialValues, submitForm, validate) => {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  //const [isSubmitted, setIsSubmitted] = useState(false);

  useEffect(() => {
    
    if (isEmpty(errors) && isSubmitting) {
      submitForm();
    }
  }, [errors, isSubmitting]);

  const handleSubmit = event => {
    if (event) {
      event.preventDefault();
    }

    const errors = validate(values);
    setErrors(errors);

    if (isEmpty(errors)) {
      setIsSubmitting(true);
    }
  };

  const validarCaptcha = captcha => {
    setValues(values => ({
      ...values,
      captcha
    }));   
  };

  const handleChange = event => {
    event.persist();
    setValues(values => ({
      ...values,
      [event.target.name]:
        event.target.type === "checkbox"
          ? event.target.checked
          : event.target.value
    }));
  };

  const finishSubmitting = () => {
     setIsSubmitting(false);
  };

  return {
    handleChange,
    validarCaptcha,
    handleSubmit,
    finishSubmitting,
    isSubmitting,
    values,
    errors
  };
};

export default useForm;
