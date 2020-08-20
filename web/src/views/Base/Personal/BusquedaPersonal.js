import React, { Component } from 'react';

import { Badge, Button, Card, CardBody, CardHeader, Col, Input, Form, Pagination, PaginationItem, PaginationLink, Row, Table, Label, FormGroup } from 'reactstrap';

class BusquedaPersonal extends Component {
    render() {
        return (
            <div className="animated fadeIn">
                <Row>
                    <Col>
                        <Card>
                            <CardHeader style={{paddingTop:'0px',paddingBottom:'0px'}}>
                                <div className="text-left mt-3 caja1">
                                <i className="fa fa-align-justify"></i>&nbsp;&nbsp;&nbsp;Búsqueda de personal
                                </div>
                                <div className="text-right mt-3 caja2">
                                    <Button color="success" className="btn-charmander">
                                        <i className="fa fa-address-book"></i>&nbsp;Añadir
                                        </Button>
                                </div>

                            </CardHeader>
                            <CardBody>
                                <Form>
                                    <FormGroup row>
                                        <Col md="6">
                                            <Label htmlFor="busq-doc">Busqueda por Documento de Identidad</Label>
                                            <Input type="text" id="busq-doc"></Input>
                                        </Col>
                                        <Col md="6">
                                            <Label htmlFor="nomb-ap">Busqueda por Nombres y Apellidos</Label>
                                            <Input type="text" id="nomb-ap"></Input>
                                        </Col>

                                    </FormGroup>
                                    <FormGroup row>
                                        <Col sm xs="12" className="text-center mt-3">
                                            <Button color="success" className="btn-square">
                                                <i className="fa fa-search"></i>&nbsp;Buscar
                                            </Button>
                                        </Col>
                                    </FormGroup>
                                </Form>
                            </CardBody>
                        </Card>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Card>
                            <CardHeader>
                                <i className="fa fa-align-justify"></i> Lista del Personal
                            </CardHeader>
                            <CardBody>
                                <Table hover bordered striped responsive size="sm">
                                    <thead>
                                        <tr>
                                            <th>Nombres y Apellidos</th>
                                            <th>Fecha de Ingreso</th>
                                            <th>Area de Trabajo</th>
                                            <th>Cargo</th>
                                            <th>Estado</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Paolo Valladares</td>
                                            <td>01/11/2019</td>
                                            <td>Equipo tecnico</td>
                                            <td>Logística</td>
                                            <td>
                                                <Badge color="success">Activo</Badge>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Stephany Carrillo</td>
                                            <td>10/02/2020</td>
                                            <td>Supervisora</td>
                                            <td>Almacén</td>
                                            <td>
                                                <Badge color="success">Activo</Badge>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Rosa Meltrozo</td>
                                            <td>07/07/2018</td>
                                            <td>Administradora</td>
                                            <td>Almacén</td>
                                            <td>
                                                <Badge color="secondary">Inactivo</Badge>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Rosa Melano</td>
                                            <td>10/01/2019</td>
                                            <td>Administradora</td>
                                            <td>Administración</td>
                                            <td>
                                                <Badge color="warning">Vacaciones</Badge>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Jhon Salchiyon</td>
                                            <td>01/01/2020</td>
                                            <td>Contador</td>
                                            <td>Presupuesto</td>
                                            <td>
                                                <Badge color="danger">Despedido</Badge>
                                            </td>
                                        </tr>
                                    </tbody>
                                </Table>
                                <nav>
                                    <Pagination>
                                        <PaginationItem><PaginationLink previous tag="button">Prev</PaginationLink></PaginationItem>
                                        <PaginationItem active>
                                            <PaginationLink tag="button">1</PaginationLink>
                                        </PaginationItem>
                                        <PaginationItem><PaginationLink tag="button">2</PaginationLink></PaginationItem>
                                        <PaginationItem><PaginationLink tag="button">3</PaginationLink></PaginationItem>
                                        <PaginationItem><PaginationLink tag="button">4</PaginationLink></PaginationItem>
                                        <PaginationItem><PaginationLink next tag="button">Next</PaginationLink></PaginationItem>
                                    </Pagination>
                                </nav>
                            </CardBody>
                        </Card>
                    </Col>
                </Row>
            </div>

        );
    }
}

export default BusquedaPersonal;
