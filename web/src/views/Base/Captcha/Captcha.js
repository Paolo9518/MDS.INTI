import React from "react";
//import ReactDOM from "react-dom";
import ReCAPTCHA from "react-google-recaptcha";

const TEST_SITE_KEY = "6LfaLsUUAAAAAEeF39JmUigTxMDNNuzi4GA8Af-u";

class Captcha extends React.Component {
  
  handleChange = value => {
    this.props.change(value);
  };
  render() {
    return <ReCAPTCHA
      sitekey={TEST_SITE_KEY}
      onChange={this.handleChange}
    />
  }
}
export default Captcha;

