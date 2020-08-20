import React, { Component } from 'react';
//Bootstrap
import {
  Button,
  Card,
  CardBody,
  CardHeader,
  Col,
  Form,
  FormGroup,
  Input,
  Label,
  Row,
  CardFooter
} from 'reactstrap';
//1º Componente 
class RegistroPersonal extends Component {
  constructor(props) {
    super(props);

    this.toggle = this.toggle.bind(this);
    this.toggleFade = this.toggleFade.bind(this);
    this.state = {
      collapse: true,
      fadeIn: true,
      timeout: 300
    };
  }

  toggle() {
    this.setState({ collapse: !this.state.collapse });
  }

  toggleFade() {
    this.setState((prevState) => { return { fadeIn: !prevState }});
  }

  render() {
    return (
      <div className="animated fadeIn">
        {/* 1º COMPONENTE - DATOS PERSONALES*/}
          {/* SE CREA UNA FILA */}
        <Row>
            {/* SE CREA UNA COLUMNA DENTRO DE UNA FILA */}
          <Col xs="12">
              {/* UNA TARJETA DENTRO DE UNA COLUMNA */}
            <Card>
              {/* UNA CABECERA DE LA TARJETA */}
              <CardHeader>
                <strong>Datos Personales</strong> 
              </CardHeader>
              {/* UN CUERPO DE LA TARJETA DONDE IRA EL FORMULARIO - FORMGROUP */}
              <CardBody>
                <Form action="" method="post" className="1-datos-pers">
                  {/* Nombres - Apellidos*/}
                  <FormGroup row>
                    <Col md="6">
                      <Label htmlFor="nombres">Nombres</Label>
                      <Input type="text" placeholder="Ingrese Nombres" />
                    </Col>
                    <Col md="6">
                      <Label htmlFor="apellidos">Apellidos</Label>
                      <Input type="text" placeholder="Ingrese Apellidos" />
                    </Col>
                  </FormGroup>
                  {/* Documentos */}
                  <FormGroup row>
                    <Col md="6">
                      <Label htmlFor="identidad">Documento de Identidad</Label>
                      <Input type="select" name="Documento de Identidad" id="Documento de Identidad">
                        <option value="1">DNI</option>
                        <option value="2">Pasaporte</option>
                        <option value="3">Carnét de extranjería</option>
                      </Input>
                    </Col>
                    <Col md="6">
                      <Label htmlFor="telefono">Celular Personal</Label>
                      <Input type="text" placeholder="Ingrese número" />
                    </Col>
                  </FormGroup>
                  {/*Fecha de Nacimiento */}
                  <FormGroup row>
                    <Col md="12">
                      <Label htmlFor="fecha">Fecha de Nacimiento</Label>
                      <Input type="date" id="fecha" name="fecha" placeholder="date-fn" />
                    </Col>
                  </FormGroup>
                </Form>
              </CardBody>
            </Card>
          </Col>
        </Row>

          {/* 2º COMPONENTES - DATOS LABORALES */}
          {/* CREA UNA FILA */}
        <Row>
          <Col>
            <Card>
              <CardHeader>
                <strong>Datos Laborales</strong>
              </CardHeader>
    
              <CardBody>
                <Form action="" method="post" className="2-datos-lab">
                  <FormGroup row>
                    <Col md="6">
                      <Label htmlFor="area">Area Laboral</Label>
                      <Input type="select" name="area" id="area">
                        <option value="1">Logistica</option>
                        <option value="2">Patrimonio</option>
                        <option value="3">Almacén</option>
                        <option value="4">Presupuesto</option>
                        <option value="5">Direccion General</option>
                        <option value="6">Comunicaciones</option>
                      </Input>

                    </Col>
                    <Col md="6">
                    
                      <Label htmlFor="cargo">Cargo Laboral</Label>
                      <Input type="select" name="cargo" id="cargo">
                        <option value="1">Jefe</option>
                        <option value="2">Secretaria</option>
                        <option value="3">Supervisor</option>
                        <option value="4">Personal</option>
                        <option value="5">Administrador</option>
                      </Input>
                    </Col>
                  </FormGroup>

                  {/* 2 Subrupo - Email y Telefono */}
                  <FormGroup row>

                    {/* Email */}
                    <Col md="6">
                      <Label htmlFor="email">E-mail</Label>
                      <Input type="text" placeholder="Ingrese E-mail" />
                    </Col>

                    {/* Telefono */}
                    <Col md="6">
                      <Label htmlFor="cel">Celular Laboral</Label>
                      <Input type="number" placeholder="Ingrese 9 digitos" />                    
                    </Col>
                  </FormGroup>

                  <FormGroup row>
                    <Col md="6">
                      <Label htmlFor="fingreso">Fecha de Ingreso</Label>
                      <Input type="date" id="fingreso" name="fingreso" placeholder="date-fin" />
                    </Col>

                    <Col md="6">
                      <Label htmlFor="num-lic">Numero de Licencia de conducir</Label>
                      <Input type="number" placeholder="Ingrese Nº de licencia"></Input>
                    </Col>
                  </FormGroup>

                </Form>
              </CardBody>
              <CardFooter>
                <div style={{textAlign:"right"}}>
                  <Button className="center-block" type="submit" size="sm" color="primary"><i className="a"></i> Submit</Button>
                  <Button className="center-block" type="reset" size="sm" color="danger"><i className="e"></i> Reset</Button>
                </div>
              </CardFooter>
            </Card>
          </Col>
        </Row>
        <FormGroup row>

       </FormGroup>
      </div>
    );
  }
}
//Exportancion
export default RegistroPersonal;
//Solo hay 1 componente + 1 bootstrap + exportacion