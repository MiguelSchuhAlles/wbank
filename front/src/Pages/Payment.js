import React, { Component } from 'react';
import { withStyles  } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Input from '@material-ui/core/Input';
  

const useStyles = theme => ({
  root: {
    '& > *': {
      margin: theme.spacing(1),
    },
  },
});

class Payment extends Component {
  constructor(props) {
    super(props);
    this.state = {
      amount: "",
      code: ""
    };

    this.handleAmountChange = this.handleAmountChange.bind(this);
    this.handleCodeChange = this.handleCodeChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleAmountChange(event) { this.setState({amount: event.target.value}); }

  handleCodeChange(event) { this.setState({code: event.target.value}); }

  handleSubmit(event) 
  {
    if(isNaN(this.state.amount)){
      alert("Invalid value! Enter a valid number.");
    }else if(this.state.amount < 0){
      alert("Invalid amount! Must be greater than zero.");
    }else if(this.state.code === ""){
        alert("Invalid data! Code can't be empty.");
    }else{
      let operationRequest = {
        accountId: this.props.accountId,
        amount: this.state.amount,
        code: this.state.code
      };

      fetch('https://localhost:44337/api/operations/ticketpayment',{
        method: "POST",
        headers: { 
          'Content-Type': 'application/json',
          authorization: "Bearer " + this.props.token
        },
        body: JSON.stringify(operationRequest),
      })
      .then(res => res.json())
      .then((data) => {
        if(data.responseStatus === 0)
          alert(`Payment executed successfully. New balance: $${data.item.balance.toFixed(2)}`);
        else
          alert(`Operation failed: ${data.message}`);
      })
      .catch(console.log)

      this.setState({amount: ""});
    }

    event.preventDefault();
  }

  render(){
    const { classes } = this.props;

    return (
      <form noValidate ref="form" onSubmit={this.handleSubmit}>
        <div>
          <Input placeholder="Amount" value={this.state.amount} onChange={this.handleAmountChange} inputProps={{ 'aria-label': 'description' }}/>
          <Input placeholder="Code" value={this.state.code} onChange={this.handleCodeChange} inputProps={{ 'aria-label': 'description' }}/>
          <Input placeholder="Description" inputProps={{ 'aria-label': 'description' }} />
        </div>
        <div className={classes.root}>
          <Button type="submit" variant="contained">Confirm Payment</Button>
        </div>
      </form>
   
    );
  }
} 

export default withStyles(useStyles)(Payment)
