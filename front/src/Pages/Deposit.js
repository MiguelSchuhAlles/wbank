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

class Deposit extends Component {
  constructor(props) {
    super(props);
    this.state = {
      amount: ""
    };

    this.handleAmountChange = this.handleAmountChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleAmountChange(event) { this.setState({amount: event.target.value}); }

  handleSubmit(event) 
  {
    if(isNaN(this.state.amount)){
      alert("Invalid value! Enter a valid number.");
    }else if(this.state.amount < 0){
      alert("Invalid amount! Must be greater than zero.");
    }else{
      let operationRequest = {
        accountId: this.props.accountId,
        amount: this.state.amount
      }

      fetch('http://localhost:5000/api/operations/deposit',{
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
          alert(`Deposit created successfully. New balance: $${data.item.balance.toFixed(2)}`);
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
          <Input placeholder="Description" inputProps={{ 'aria-label': 'description' }} />
        </div>
        <div className={classes.root}>
          <Button type="submit" variant="contained">Confirm Deposit</Button>
        </div>
      </form>
   
    );
  }
} 

export default withStyles(useStyles)(Deposit)
