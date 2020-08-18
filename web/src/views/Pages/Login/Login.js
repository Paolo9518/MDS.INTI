import React, { useContext, useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Button, Card, CardBody, CardGroup, Col, Container, FormFeedback, Input, InputGroup, InputGroupAddon, InputGroupText, Row } from 'reactstrap';
import { Formik, Form } from 'formik';
import * as Yup from "yup";
import './ValidationForms.css'
import TextField from "../../Base/CustomInput/TextField";

const validationSchema = function (values) {
  return Yup.object().shape({
    usuario: Yup.string()
      .min(2, `First name has to be at least 2 characters`)
      .required('First name is required'),
    password: Yup.string()
      .min(6, `Password has to be at least ${6} characters!`)
      .matches(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/, 'Password must contain: numbers, uppercase and lowercase letters\n')
      .required('Password is required'),
  })
}

const validate = (getValidationSchema) => {
  return (values) => {
    const validationSchema = getValidationSchema(values)
    try {
      validationSchema.validateSync(values, { abortEarly: false })
      return {}
    } catch (error) {
      return getErrorsFromValidationError(error)
    }
  }
}

const getErrorsFromValidationError = (validationError) => {
  const FIRST_ERROR = 0
  return validationError.inner.reduce((errors, error) => {
    return {
      ...errors,
      [error.path]: error.errors[FIRST_ERROR],
    }
  }, {})
}

const initialValues = {
  usuario: "",
  password: "",
}

const onSubmit = (values, { setSubmitting, setErrors }) => {
  setTimeout(() => {
    
    alert(JSON.stringify(values, null, 2));
    setSubmitting(false)
  }, 2000)
}

/* Contrucción del Interfaz de Login */
const Login = (props) => {

  const findFirstError = (formName, hasError) => {
    const form = document.forms[formName]
    for (let i = 0; i < form.length; i++) {
      if (hasError(form[i].name)) {
        form[i].focus()
        break
      }
    }
  }

  const validateForm = (errors) => {
    findFirstError('formLogin', (fieldName) => {
      return Boolean(errors[fieldName])
    })
  }

  const touchAll = (setTouched, errors) => {
    setTouched({
      usuario: true,
      password: true
    }
    )
    validateForm(errors)
  }

  return (
    <div className="app flex-row align-items-center">
      <Container>
        <Row className="justify-content-center">
          <Col md="4">
            <CardGroup>
              <Card className="p-4">
                <CardBody>
                  <Formik
                    initialValues={initialValues}
                    validate={validate(validationSchema)}
                    onSubmit={onSubmit}
                  >
                    {({
                      values,
                      errors,
                      touched,
                      status,
                      dirty,
                      handleChange,
                      handleBlur,
                      handleSubmit,
                      isSubmitting,
                      isValid,
                      handleReset,
                      setTouched
                    }) => {
                      return (
                        <Form onSubmit={handleSubmit} noValidate name='formLogin'>
                          <h2>ACCESO</h2>
                          <p className="text-muted">Ingrese con su cuenta</p>
                          <InputGroup className="mb-3">
                            <InputGroupAddon addonType="prepend">
                              <InputGroupText>
                                <i className="icon-user"></i>
                              </InputGroupText>
                            </InputGroupAddon>
                            <Input type="text"
                              name="usuario"
                              id="usuario"
                              placeholder="Usuario"
                              autoComplete="given-name"
                              valid={!errors.usuario}
                              invalid={touched.usuario && !!errors.usuario}
                              autoFocus={true}
                              required
                              onChange={handleChange}
                              onBlur={handleBlur}
                              value={values.usuario} />
                            <FormFeedback>{errors.usuario}</FormFeedback>
                          </InputGroup>
                          <InputGroup className="mb-4">
                            <InputGroupAddon addonType="prepend">
                              <InputGroupText>
                                <i className="icon-lock"></i>
                              </InputGroupText>
                            </InputGroupAddon>
                            <Input type="password"
                              name="password"
                              id="password"
                              placeholder="Contraseña"
                              autoComplete="new-password"
                              valid={!errors.password}
                              invalid={touched.password && !!errors.password}
                              required
                              onChange={handleChange}
                              onBlur={handleBlur}
                              value={values.password} />
                            <FormFeedback>{errors.password}</FormFeedback>
                          </InputGroup>
                          <Row>
                            <Col xs="6">
                              <Button type="submit" color="primary" className="px-4" onClick={() => touchAll(setTouched, errors)} disabled={isSubmitting || !isValid} >Login</Button>
                            </Col>
                          </Row>
                        </Form>
                      );
                    }}
                  </Formik>
                </CardBody>
              </Card>
            </CardGroup>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default Login;
