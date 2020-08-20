import React from 'react';
import { Input } from 'reactstrap';
import { ErrorMessage, Field } from "formik";

const TextField  = ({ name, label, type = "text", required = false}) => {
  return (
      <Field
        required={required}
        autoComplete="off"
        as={Input}
        label={label}
        name={name}
        fullWidth
        type={type}
        helperText={<ErrorMessage name={name} />}
      />
  );
};

export default TextField;