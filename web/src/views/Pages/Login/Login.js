import React, { useContext, useState, useEffect } from 'react';
import { Button, Card, CardBody, CardGroup, Col, Container, Form, FormFeedback, Input, InputGroup, InputGroupAddon, InputGroupText, Row } from 'reactstrap';
import './ValidationForms.css';
import useForm from "./../../../hooks/useForm";
import { AuthContext } from "./../../../context/index";
import Captcha from "../../Base/Captcha/Captcha";
const { isFormValid } = require("./../../../utils/validate");

const initialValues = {
  email: "",
  password: "",
  keepLoggedIn: false,
  //captcha: "", // captcha bypass
  expired: false
};

const Login = (props) => {
  const [show, setShow] = useState(false);
  const [msg, setMsg] = useState('');

  const {
    handleChange,
    handleSubmit,
    //finishSubmitting,
    //isSubmitting,
    values,
    errors,
    //isSubmitted,
    validarCaptcha
  } = useForm(initialValues, () => submitForm(), isFormValid);

  const { loading, token, login } = useContext(AuthContext);

  useEffect(() => {
    if (errors.captcha === '' || errors.captcha == undefined) {
      setMsg('');
      setShow(false);
    } else {
      setMsg(errors.captcha);
      setShow(true);
    }
  }, [errors]);

  const submitForm = async () => {
    debugger;
    const token = await login(values);
    debugger;
    if (!token.data.data.token) {
      setMsg('');
      setShow(false);

    } else {
      values.email = "";
      values.password = "";
      setMsg(token.data.messages[0]);
      setShow(true);
    }
  }

  useEffect(() => {
    debugger;
    if (token !== "")
      if (token.data.success) {
        props.history.push('/admin')
      }
  }, [token])

  const goForm = event => {
    event.preventDefault();
    handleSubmit();
  }

  return (
    <div className="app flex-row align-items-center">
      <Container>
        <Row className="justify-content-center">
          <Col md="4">
            <CardGroup>
              <Card className="p-4">
                <CardBody>
                  <Form onSubmit={handleSubmit} noValidate name='formLogin'>
                    <h2>ACCESO</h2>
                    <p className="text-muted">Ingrese con su cuenta</p>
                    <InputGroup className="mb-3">
                      <InputGroupAddon addonType="prepend">
                        <InputGroupText>
                          <i className="icon-user"></i>
                        </InputGroupText>
                      </InputGroupAddon>
                      <Input name="email" type="text" required autoComplete="username"
                        value={values.email} onChange={handleChange} placeholder="Usuario"
                        autoCorrect="off" autoCapitalize="off" className="input-login" />
                      <FormFeedback>{errors.usuario}</FormFeedback>
                    </InputGroup>

                    <InputGroup className="mb-4">
                      <InputGroupAddon addonType="prepend">
                        <InputGroupText>
                          <i className="icon-lock"></i>
                        </InputGroupText>
                      </InputGroupAddon>
                      <Input name="password" type="password" required autoComplete="current-password"
                        value={values.password} onChange={handleChange} placeholder="Contraseña"
                        autoCorrect="off" autoCapitalize="off" className="input-login" />
                      <FormFeedback>{errors.password}</FormFeedback>
                    </InputGroup>

                    <InputGroup className="mb-5">
                      <Captcha id="stiloscaptcha" change={validarCaptcha} />
                    </InputGroup>

                    <Row>
                      <Col xs="6">
                        <Button type="submit" color="primary" className="px-4" onClick={(e) => goForm(e)} >Login</Button>
                      </Col>
                    </Row>
                  </Form>
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
